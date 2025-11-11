using ActiveControlApi.DTO.Manutencao;
using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.Services.Manutencao
{
    public interface IManutencaoService
    {
        // CRUD básico
        Task<IEnumerable<ManutencaoDTO>> PegarTodos();
        Task<ManutencaoDTO> PegarPorId(int idManutencao);
        Task<ManutencaoDTO> CriarManutencao(ManutencaoDTO manutencaoRegistro);
        Task<ManutencaoDTO> AtualizarManutencao(int idManutencao, ManutencaoDTO manutencaoRegistro);
        Task<bool> RemoverManutencao(int idManutencao);

        // Filtros avançados
        Task<IEnumerable<ManutencaoDTO>> BuscarPorStatus(StatusManutencao status);
        Task<IEnumerable<ManutencaoDTO>> BuscarPorTipo(TipoManutencao tipo);
        Task<IEnumerable<ManutencaoDTO>> BuscarPorPrioridade(PrioridadeSolicitacao prioridade);
        Task<IEnumerable<ManutencaoDTO>> BuscarPorUsuarioResponsavel(int usuarioId);
        Task<IEnumerable<ManutencaoDTO>> BuscarAtrasadas();
        Task<IEnumerable<ManutencaoDTO>> BuscarPorPeriodo(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<ManutencaoDTO>> BuscarAgendadas();

        // Workflow
        Task<ManutencaoDTO> AtribuirResponsavel(int manutencaoId, int usuarioResponsavelId);
        Task<ManutencaoDTO> Agendar(int manutencaoId, DateTime dataAgendada);
        Task<ManutencaoDTO> Iniciar(int manutencaoId);
        Task<ManutencaoDTO> Concluir(int manutencaoId, string? solucaoAplicada = null, decimal? custoReal = null);
        Task<ManutencaoDTO> Cancelar(int manutencaoId, string? motivo = null);
        Task<ManutencaoDTO> AlterarPrioridade(int manutencaoId, PrioridadeSolicitacao prioridade);

        // Relatórios
        Task<Dictionary<string, object>> ObterEstatisticas();
        Task<IEnumerable<ManutencaoDTO>> BuscarManutencoesAtribuidas(int usuarioId);
    }
}



