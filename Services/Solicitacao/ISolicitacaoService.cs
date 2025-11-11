using ActiveControlApi.DTO.Solicitacao;
using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.Services.Solicitacao
{
    public interface ISolicitacaoService
    {
        // CRUD básico
        Task<IEnumerable<SolicitacaoDTO>> PegarTodos();
        Task<SolicitacaoDTO> PegarPorId(int idSolicitacao);
        Task<SolicitacaoDTO> CriarSolicitacao(SolicitacaoDTO solicitacaoRegistro);
        Task<SolicitacaoDTO> AtualizarSolicitacao(int idSolicitacao, SolicitacaoDTO solicitacaoRegistro);
        Task<bool> RemoverSolicitacao(int idSolicitacao);

        // Filtros avançados
        Task<IEnumerable<SolicitacaoDTO>> BuscarPorStatus(StatusSolicitacao status);
        Task<IEnumerable<SolicitacaoDTO>> BuscarPorTipo(TipoSolicitacao tipo);
        Task<IEnumerable<SolicitacaoDTO>> BuscarPorPrioridade(PrioridadeSolicitacao prioridade);
        Task<IEnumerable<SolicitacaoDTO>> BuscarPorUsuarioSolicitante(int usuarioId);
        Task<IEnumerable<SolicitacaoDTO>> BuscarPorUsuarioResponsavel(int usuarioId);
        Task<IEnumerable<SolicitacaoDTO>> BuscarPorAtivo(int ativoId);
        Task<IEnumerable<SolicitacaoDTO>> BuscarAtrasadas();
        Task<IEnumerable<SolicitacaoDTO>> BuscarPorPeriodo(DateTime dataInicio, DateTime dataFim);

        // Workflow
        Task<SolicitacaoDTO> AtribuirResponsavel(int solicitacaoId, int usuarioResponsavelId);
        Task<SolicitacaoDTO> IniciarAtendimento(int solicitacaoId);
        Task<SolicitacaoDTO> Finalizar(int solicitacaoId, string? observacao = null);
        Task<SolicitacaoDTO> Cancelar(int solicitacaoId, string? motivo = null);
        Task<SolicitacaoDTO> AlterarPrioridade(int solicitacaoId, PrioridadeSolicitacao prioridade);

        // Relatórios e estatísticas
        Task<Dictionary<string, object>> ObterEstatisticas();
        Task<Dictionary<string, object>> ObterEstatisticasPorPeriodo(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<SolicitacaoDTO>> BuscarMinhasSolicitacoes(int usuarioId);
        Task<IEnumerable<SolicitacaoDTO>> BuscarSolicitacoesAtribuidas(int usuarioId);
    }
}



