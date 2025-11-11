using ActiveControlApi.DTO.Historico;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Models;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Services.Historico
{
    public class HistoricoService : IHistoricoService
    {
        private readonly IUnitOfWork _uow;

        public HistoricoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterHistoricoPorAtivo(int ativoId)
        {
            var historicos = await _uow.HistoricoMovimentacao.GetQueryble()
                .Where(h => h.AtivoId == ativoId)
                .Include(x => x.Ativo)
                .Include(x => x.Usuario)
                .Include(x => x.Departamento)
                .Include(x => x.UsuarioResponsavel)
                .Include(x => x.Solicitacao)
                .OrderByDescending(h => h.DataMovimentacao)
                .ToListAsync();

            return historicos.ParaListaDto();
        }

        public async Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterHistoricoPorUsuario(int usuarioId)
        {
            var historicos = await _uow.HistoricoMovimentacao.GetQueryble()
                .Where(h => h.UsuarioId == usuarioId || h.UsuarioResponsavelId == usuarioId)
                .Include(x => x.Ativo)
                .Include(x => x.Usuario)
                .Include(x => x.Departamento)
                .Include(x => x.UsuarioResponsavel)
                .Include(x => x.Solicitacao)
                .OrderByDescending(h => h.DataMovimentacao)
                .ToListAsync();

            return historicos.ParaListaDto();
        }

        public async Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterHistoricoPorTipo(TipoMovimentacao tipo)
        {
            var historicos = await _uow.HistoricoMovimentacao.GetQueryble()
                .Where(h => h.TipoMovimentacao == tipo)
                .Include(x => x.Ativo)
                .Include(x => x.Usuario)
                .Include(x => x.Departamento)
                .Include(x => x.UsuarioResponsavel)
                .Include(x => x.Solicitacao)
                .OrderByDescending(h => h.DataMovimentacao)
                .ToListAsync();

            return historicos.ParaListaDto();
        }

        public async Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterHistoricoPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var historicos = await _uow.HistoricoMovimentacao.GetQueryble()
                .Where(h => h.DataMovimentacao >= dataInicio && h.DataMovimentacao <= dataFim)
                .Include(x => x.Ativo)
                .Include(x => x.Usuario)
                .Include(x => x.Departamento)
                .Include(x => x.UsuarioResponsavel)
                .Include(x => x.Solicitacao)
                .OrderByDescending(h => h.DataMovimentacao)
                .ToListAsync();

            return historicos.ParaListaDto();
        }

        public async Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterTodos()
        {
            var historicos = await _uow.HistoricoMovimentacao.GetQueryble()
                .Include(x => x.Ativo)
                .Include(x => x.Usuario)
                .Include(x => x.Departamento)
                .Include(x => x.UsuarioResponsavel)
                .Include(x => x.Solicitacao)
                .OrderByDescending(h => h.DataMovimentacao)
                .ToListAsync();

            return historicos.ParaListaDto();
        }

        public async Task<HistoricoMovimentacaoDTO> ObterPorId(int id)
        {
            var historico = await _uow.HistoricoMovimentacao.GetQueryble()
                .Where(h => h.Id == id)
                .Include(x => x.Ativo)
                .Include(x => x.Usuario)
                .Include(x => x.Departamento)
                .Include(x => x.UsuarioResponsavel)
                .Include(x => x.Solicitacao)
                .FirstOrDefaultAsync();

            if (historico == null)
                throw new KeyNotFoundException($"Histórico com id {id} não encontrado.");

            return historico.ParaDto();
        }

        public async Task RegistrarMovimentacao(
            TipoMovimentacao tipo,
            int ativoId,
            int usuarioResponsavelId,
            string descricao,
            int? usuarioId = null,
            int? departamentoId = null,
            int? solicitacaoId = null,
            string? dadosAnteriores = null,
            string? dadosNovos = null,
            string? ipAddress = null)
        {
            var historico = new HistoricoMovimentacao
            {
                TipoMovimentacao = tipo,
                AtivoId = ativoId,
                UsuarioId = usuarioId,
                DepartamentoId = departamentoId,
                SolicitacaoId = solicitacaoId,
                Descricao = descricao,
                DadosAnteriores = dadosAnteriores,
                DadosNovos = dadosNovos,
                UsuarioResponsavelId = usuarioResponsavelId,
                DataMovimentacao = DateTime.UtcNow,
                IpAddress = ipAddress
            };

            _uow.HistoricoMovimentacao.Create(historico);
            await _uow.CommitAsync();
        }
    }
}

