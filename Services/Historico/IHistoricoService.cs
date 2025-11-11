using ActiveControlApi.DTO.Historico;
using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.Services.Historico
{
    public interface IHistoricoService
    {
        Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterHistoricoPorAtivo(int ativoId);
        Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterHistoricoPorUsuario(int usuarioId);
        Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterHistoricoPorTipo(TipoMovimentacao tipo);
        Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterHistoricoPorPeriodo(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<HistoricoMovimentacaoDTO>> ObterTodos();
        Task<HistoricoMovimentacaoDTO> ObterPorId(int id);
        Task RegistrarMovimentacao(
            TipoMovimentacao tipo,
            int ativoId,
            int usuarioResponsavelId,
            string descricao,
            int? usuarioId = null,
            int? departamentoId = null,
            int? solicitacaoId = null,
            string? dadosAnteriores = null,
            string? dadosNovos = null,
            string? ipAddress = null);
    }
}


