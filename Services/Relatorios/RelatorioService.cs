using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Services.Relatorios
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IUnitOfWork _uow;

        public RelatorioService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<object> QuantidadeAtivos(string groupBy)
        {
            var query = _uow.Ativo.GetQueryble();

            if (string.Equals(groupBy, "categoria", StringComparison.OrdinalIgnoreCase))
            {
                var dados = await query
                    .GroupBy(a => a.CategoriaAtivoId)
                    .Select(g => new { CategoriaId = g.Key, Quantidade = g.Count() })
                    .ToListAsync();
                return new { Tipo = "Categoria", Dados = dados };
            }

            if (string.Equals(groupBy, "departamento", StringComparison.OrdinalIgnoreCase))
            {
                var ativosDepto = await _uow.AtivoDepartamento.GetQueryble()
                    .Where(ad => ad.DataFim == null)
                    .GroupBy(ad => ad.DepartamentoId)
                    .Select(g => new { DepartamentoId = g.Key, Quantidade = g.Count() })
                    .ToListAsync();
                return new { Tipo = "Departamento", Dados = ativosDepto };
            }

            var total = await query.CountAsync();
            return new { Tipo = "Total", Dados = new[] { new { Total = total } } };
        }

        public async Task<IEnumerable<object>> AtivosPorProfissional()
        {
            var dados = await _uow.AtivoUsuario.GetQueryble()
                .Where(au => au.DataFim == null)
                .GroupBy(au => au.UsuarioId)
                .Select(g => new { UsuarioId = g.Key, Quantidade = g.Count() })
                .ToListAsync();
            return dados.Cast<object>();
        }

        public async Task<IEnumerable<object>> UsuariosAtivos()
        {
            var dados = await _uow.Usuario.GetQueryble()
                .Select(u => new { u.Id, u.NomeCompleto, u.Email })
                .ToListAsync();
            return dados.Cast<object>();
        }

        public async Task<IEnumerable<object>> CustoPorAtivo()
        {
            // custo = aquisição + soma de manutenções reais (se houver) — aqui apenas aquisição e custo real de manutenções
            var ativos = await _uow.Ativo.GetQueryble()
                .Select(a => new { a.Id, a.AtivoNome, a.ValorAquisicao })
                .ToListAsync();

            var manutencoes = await _uow.Manutencao.GetQueryble()
                .Include(m => m.Solicitacao)
                .Where(m => m.CustoReal.HasValue)
                .GroupBy(m => m.Solicitacao.AtivoId)
                .Select(g => new { AtivoId = g.Key, CustoReal = g.Sum(x => x.CustoReal) })
                .ToListAsync();

            var custoPorAtivo = from a in ativos
                                join m in manutencoes on a.Id equals m.AtivoId into gj
                                from mm in gj.DefaultIfEmpty()
                                select new
                                {
                                    AtivoId = a.Id,
                                    a.AtivoNome,
                                    CustoAquisicao = a.ValorAquisicao,
                                    CustoManutencaoReal = mm?.CustoReal ?? 0m,
                                    CustoTotal = a.ValorAquisicao + (mm?.CustoReal ?? 0m)
                                };

            return custoPorAtivo.Cast<object>().ToList();
        }

        public async Task<IEnumerable<object>> ManutencaoPreventiva()
        {
            var preventivas = await _uow.Manutencao.GetQueryble()
                .Where(m => m.TipoManutencao == Models.Enums.TipoManutencao.Preventiva)
                .Select(m => new
                {
                    m.Id,
                    m.SolicitacaoId,
                    m.StatusManutencao,
                    m.DataAgendada,
                    m.DataCriacao,
                    m.DataPrazo
                })
                .ToListAsync();

            return preventivas.Cast<object>();
        }
    }
}









