using ActiveControlApi.DTO.Incidente;
using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.Services.Incidente
{
    public interface IIncidenteService
    {
        // CRUD básico
        Task<IEnumerable<IncidenteDTO>> PegarTodos();
        Task<IncidenteDTO> PegarPorId(int idIncidente);
        Task<IncidenteDTO> CriarIncidente(IncidenteDTO incidenteRegistro);
        Task<IncidenteDTO> AtualizarIncidente(int idIncidente, IncidenteDTO incidenteRegistro);
        Task<bool> RemoverIncidente(int idIncidente);

        // Filtros avançados
        Task<IEnumerable<IncidenteDTO>> BuscarPorStatus(StatusIncidente status);
        Task<IEnumerable<IncidenteDTO>> BuscarPorSeveridade(SeveridadeIncidente severidade);
        Task<IEnumerable<IncidenteDTO>> BuscarPorPrioridade(PrioridadeSolicitacao prioridade);
        Task<IEnumerable<IncidenteDTO>> BuscarPorUsuarioResponsavel(int usuarioId);
        Task<IEnumerable<IncidenteDTO>> BuscarAbertos();
        Task<IEnumerable<IncidenteDTO>> BuscarAtrasados();
        Task<IEnumerable<IncidenteDTO>> BuscarPorPeriodo(DateTime dataInicio, DateTime dataFim);

        // Workflow
        Task<IncidenteDTO> AtribuirResponsavel(int incidenteId, int usuarioResponsavelId);
        Task<IncidenteDTO> IniciarAnalise(int incidenteId);
        Task<IncidenteDTO> IniciarResolucao(int incidenteId);
        Task<IncidenteDTO> Resolver(int incidenteId, string? solucaoAplicada = null, string? causaRaiz = null);
        Task<IncidenteDTO> Cancelar(int incidenteId, string? motivo = null);
        Task<IncidenteDTO> AlterarPrioridade(int incidenteId, PrioridadeSolicitacao prioridade);
        Task<IncidenteDTO> AlterarSeveridade(int incidenteId, SeveridadeIncidente severidade);

        // Relatórios
        Task<Dictionary<string, object>> ObterEstatisticas();
        Task<IEnumerable<IncidenteDTO>> BuscarIncidentesAtribuidos(int usuarioId);
    }
}



