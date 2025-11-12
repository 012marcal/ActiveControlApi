using ActiveControlApi.DTO.Ativo;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;
using StatusSolicitacao = ActiveControlApi.Models.Enums.StatusSolicitacao;

namespace ActiveControlApi.Services.Ativo
{
    public class AtivoService : IAtivoService
    {
        private readonly IUnitOfWork _uow;

        public AtivoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<AtivoDTO>> PegarTodos()
        {
            var ativos = await _uow.Ativo.GetAll();
            if (!ativos.Any())
                return Enumerable.Empty<AtivoDTO>();

            return ativos.ParaListaAtivoDto();
        }

        public async Task<AtivoDTO> PegarPorId(int idAtivo)
        {
            var ativo = await _uow.Ativo.Get(a => a.Id == idAtivo);
            if (ativo == null)
                throw new KeyNotFoundException($"Ativo com id {idAtivo} não encontrado.");

            return ativo.ParaAtivoDto();
        }

        public async Task<AtivoDTO> CriarAtivo(AtivoDTO ativoRegistro)
        {
            // Validações de negócio
            var patrimonioExistente = await _uow.Ativo.Get(a => a.NumPatrimonio == ativoRegistro.NumPatrimonio);
            if (patrimonioExistente != null)
                throw new InvalidOperationException($"Já existe um ativo com o número de patrimônio {ativoRegistro.NumPatrimonio}.");

            var serieExistente = await _uow.Ativo.Get(a => a.NumSerie == ativoRegistro.NumSerie);
            if (serieExistente != null)
                throw new InvalidOperationException($"Já existe um ativo com o número de série {ativoRegistro.NumSerie}.");

            // Validar se modelo existe
            var modelo = await _uow.ModeloAtivo.Get(m => m.Id == ativoRegistro.ModeloAtivoId);
            if (modelo == null)
                throw new KeyNotFoundException($"Modelo de ativo com id {ativoRegistro.ModeloAtivoId} não encontrado.");

            // Validar se categoria existe
            var categoria = await _uow.CategoriaAtivo.Get(c => c.Id == ativoRegistro.CategoriaAtivoId);
            if (categoria == null)
                throw new KeyNotFoundException($"Categoria de ativo com id {ativoRegistro.CategoriaAtivoId} não encontrada.");

            var entity = ativoRegistro.ParaAtivo();
            if (entity == null)
                throw new ArgumentNullException(nameof(ativoRegistro), "Não foi possível criar o ativo.");

            // Definir status padrão se não informado
            if (!entity.StatusAtivo.HasValue)
                entity.StatusAtivo = statusAtivo.Disponivel;

            var criado = _uow.Ativo.Create(entity);
            await _uow.CommitAsync();

            return criado.ParaAtivoDto();
        }

        public async Task<AtivoDTO> AtualizarAtivo(int idAtivo, AtivoDTO ativoRegistro)
        {
            var entity = await _uow.Ativo.Get(a => a.Id == idAtivo);
            if (entity == null)
                throw new KeyNotFoundException($"Ativo com id {idAtivo} não encontrado.");

            if (!string.IsNullOrWhiteSpace(ativoRegistro.AtivoNome))
                entity.AtivoNome = ativoRegistro.AtivoNome;
            if (!string.IsNullOrWhiteSpace(ativoRegistro.NumPatrimonio))
                entity.NumPatrimonio = ativoRegistro.NumPatrimonio;
            if (!string.IsNullOrWhiteSpace(ativoRegistro.NumSerie))
                entity.NumSerie = ativoRegistro.NumSerie;

            if (ativoRegistro.ValorAquisicao > 0)
                entity.ValorAquisicao = ativoRegistro.ValorAquisicao;
            if (ativoRegistro.DataAquisicao != default)
                entity.DataAquisicao = ativoRegistro.DataAquisicao;
            if (ativoRegistro.StatusAtivo.HasValue)
                entity.StatusAtivo = (Models.Enums.statusAtivo)ativoRegistro.StatusAtivo.Value;

            if (ativoRegistro.ModeloAtivoId > 0)
                entity.ModeloAtivoId = ativoRegistro.ModeloAtivoId;
            if (ativoRegistro.CategoriaAtivoId > 0)
                entity.CategoriaAtivoId = ativoRegistro.CategoriaAtivoId;

            if (ativoRegistro.VidaUtilEstimadaAnos > 0)
                entity.VidaUtilEstimadaAnos = ativoRegistro.VidaUtilEstimadaAnos;
            if (ativoRegistro.TaxaDepreciacaoAnual > 0)
                entity.TaxaDepreciacaoAnual = ativoRegistro.TaxaDepreciacaoAnual;

            _uow.Ativo.Update(entity);
            await _uow.CommitAsync();

            return entity.ParaAtivoDto();
        }

        public async Task<bool> RemoverAtivo(int idAtivo)
        {
            var entity = await _uow.Ativo.Get(a => a.Id == idAtivo);
            if (entity == null)
                throw new KeyNotFoundException($"Ativo com id {idAtivo} não encontrado.");

            // Validar se há alocações ativas
            var alocacaoUsuario = await _uow.AtivoUsuario.Any(x => x.AtivoId == idAtivo && x.DataFim == null);
            var alocacaoDepartamento = await _uow.AtivoDepartamento.Any(x => x.AtivoId == idAtivo && x.DataFim == null);
            if (alocacaoUsuario || alocacaoDepartamento)
                throw new InvalidOperationException("Não é possível remover um ativo que possui alocações ativas.");

            // Validar se há solicitações abertas
            var solicitacaoAberta = await _uow.Solicitacao.Any(s => s.AtivoId == idAtivo && 
                (s.StatusSolicitacao == StatusSolicitacao.ABERTA || s.StatusSolicitacao == StatusSolicitacao.EM_ANDAMENTO));
            if (solicitacaoAberta)
                throw new InvalidOperationException("Não é possível remover um ativo que possui solicitações em aberto.");

            _uow.Ativo.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }

        // Consultas avançadas
        public async Task<IEnumerable<AtivoDTO>> BuscarPorStatus(statusAtivo status)
        {
            var ativos = await _uow.Ativo.GetAll(a => a.StatusAtivo == status);
            if (!ativos.Any())
                return Enumerable.Empty<AtivoDTO>();

            return ativos.ParaListaAtivoDto();
        }

        public async Task<IEnumerable<AtivoDTO>> BuscarPorCategoria(int categoriaId)
        {
            var ativos = await _uow.Ativo.GetAll(a => a.CategoriaAtivoId == categoriaId);
            if (!ativos.Any())
                return Enumerable.Empty<AtivoDTO>();

            return ativos.ParaListaAtivoDto();
        }

        public async Task<IEnumerable<AtivoDTO>> BuscarPorModelo(int modeloId)
        {
            var ativos = await _uow.Ativo.GetAll(a => a.ModeloAtivoId == modeloId);
            if (!ativos.Any())
                return Enumerable.Empty<AtivoDTO>();

            return ativos.ParaListaAtivoDto();
        }

        public async Task<IEnumerable<AtivoDTO>> BuscarDisponiveis()
        {
            var ativos = await _uow.Ativo.GetAll(a => a.StatusAtivo == statusAtivo.Disponivel);
            if (!ativos.Any())
                return Enumerable.Empty<AtivoDTO>();

            return ativos.ParaListaAtivoDto();
        }

        public async Task<IEnumerable<AtivoDTO>> BuscarEmManutencao()
        {
            var ativos = await _uow.Ativo.GetAll(a => a.StatusAtivo == statusAtivo.Manutencao);
            if (!ativos.Any())
                return Enumerable.Empty<AtivoDTO>();

            return ativos.ParaListaAtivoDto();
        }

        public async Task<AtivoDTO> BuscarPorNumeroPatrimonio(string numPatrimonio)
        {
            var ativo = await _uow.Ativo.Get(a => a.NumPatrimonio == numPatrimonio);
            if (ativo == null)
                throw new KeyNotFoundException($"Ativo com número de patrimônio {numPatrimonio} não encontrado.");

            return ativo.ParaAtivoDto();
        }

        public async Task<AtivoDTO> BuscarPorNumeroSerie(string numSerie)
        {
            var ativo = await _uow.Ativo.Get(a => a.NumSerie == numSerie);
            if (ativo == null)
                throw new KeyNotFoundException($"Ativo com número de série {numSerie} não encontrado.");

            return ativo.ParaAtivoDto();
        }

        // Depreciação
        public async Task<decimal> CalcularDepreciacao(int idAtivo)
        {
            var ativo = await _uow.Ativo.Get(a => a.Id == idAtivo);
            if (ativo == null)
                throw new KeyNotFoundException($"Ativo com id {idAtivo} não encontrado.");

            var anosDecorridos = (DateTime.UtcNow - ativo.DataAquisicao).TotalDays / 365.25;
            var anosDecorridosInt = Math.Max(0, (int)anosDecorridos);

            if (anosDecorridosInt >= ativo.VidaUtilEstimadaAnos)
                return 0; // Ativo totalmente depreciado

            var depreciacaoAnual = ativo.ValorAquisicao * (ativo.TaxaDepreciacaoAnual / 100m);
            var depreciacaoTotal = depreciacaoAnual * anosDecorridosInt;
            var valorDepreciado = Math.Max(0, ativo.ValorAquisicao - depreciacaoTotal);

            return valorDepreciado;
        }

        public async Task<IEnumerable<AtivoDTO>> ListarProximosVencimento(int mesesAntecedencia = 6)
        {
            var dataLimite = DateTime.UtcNow.AddMonths(mesesAntecedencia);
            var todosAtivos = await _uow.Ativo.GetAll();

            var ativosProximosVencimento = todosAtivos.Where(a =>
            {
                var dataVencimento = a.DataAquisicao.AddYears(a.VidaUtilEstimadaAnos);
                return dataVencimento <= dataLimite && dataVencimento >= DateTime.UtcNow;
            }).ToList();

            if (!ativosProximosVencimento.Any())
                return Enumerable.Empty<AtivoDTO>();

            return ativosProximosVencimento.ParaListaAtivoDto();
        }

        // Relatórios
        public async Task<Dictionary<string, object>> ObterEstatisticas()
        {
            var todosAtivos = await _uow.Ativo.GetAll();
            var totalAtivos = todosAtivos.Count();
            var valorTotal = todosAtivos.Sum(a => a.ValorAquisicao);
            var disponiveis = todosAtivos.Count(a => a.StatusAtivo == statusAtivo.Disponivel);
            var emUso = todosAtivos.Count(a => a.StatusAtivo == statusAtivo.EmUso);
            var emManutencao = todosAtivos.Count(a => a.StatusAtivo == statusAtivo.Manutencao);

            // Calcular valor depreciado total
            decimal valorDepreciadoTotal = 0;
            foreach (var ativo in todosAtivos)
            {
                var anosDecorridos = (DateTime.UtcNow - ativo.DataAquisicao).TotalDays / 365.25;
                var anosDecorridosInt = Math.Max(0, (int)anosDecorridos);
                if (anosDecorridosInt < ativo.VidaUtilEstimadaAnos)
                {
                    var depreciacaoAnual = ativo.ValorAquisicao * (ativo.TaxaDepreciacaoAnual / 100m);
                    var depreciacaoTotal = depreciacaoAnual * anosDecorridosInt;
                    valorDepreciadoTotal += Math.Max(0, ativo.ValorAquisicao - depreciacaoTotal);
                }
            }

            return new Dictionary<string, object>
            {
                { "TotalAtivos", totalAtivos },
                { "ValorTotalAquisicao", valorTotal },
                { "ValorDepreciadoTotal", valorDepreciadoTotal },
                { "Disponiveis", disponiveis },
                { "EmUso", emUso },
                { "EmManutencao", emManutencao },
                { "PorCategoria", todosAtivos.GroupBy(a => a.CategoriaAtivoId).Select(g => new { CategoriaId = g.Key, Quantidade = g.Count() }).ToList() }
            };
        }

        public async Task<IEnumerable<AtivoDTO>> BuscarPorUsuario(int usuarioId)
        {
            var alocacoes = await _uow.AtivoUsuario.GetAll(au => au.UsuarioId == usuarioId && au.DataFim == null);
            if (!alocacoes.Any())
                return Enumerable.Empty<AtivoDTO>();

            var ativoIds = alocacoes.Select(a => a.AtivoId).ToList();
            var ativos = await _uow.Ativo.GetAll(a => ativoIds.Contains(a.Id));

            return ativos.ParaListaAtivoDto();
        }

        public async Task<IEnumerable<AtivoDTO>> BuscarPorDepartamento(int departamentoId)
        {
            var alocacoes = await _uow.AtivoDepartamento.GetAll(ad => ad.DepartamentoId == departamentoId && ad.DataFim == null);
            if (!alocacoes.Any())
                return Enumerable.Empty<AtivoDTO>();

            var ativoIds = alocacoes.Select(a => a.AtivoId).ToList();
            var ativos = await _uow.Ativo.GetAll(a => ativoIds.Contains(a.Id));

            return ativos.ParaListaAtivoDto();
        }

        // Paginação
        public async Task<(IEnumerable<AtivoDTO> itens, int total)> PegarPaginado(int pagina, int tamanhoPagina)
        {
            pagina = pagina <= 0 ? 1 : pagina;
            tamanhoPagina = tamanhoPagina <= 0 ? 10 : Math.Min(tamanhoPagina, 100);

            var query = _uow.Ativo.GetQueryble();
            var total = await query.CountAsync();

            var itens = await query
                .OrderBy(a => a.Id)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (itens.ParaListaAtivoDto(), total);
        }
    }
}


