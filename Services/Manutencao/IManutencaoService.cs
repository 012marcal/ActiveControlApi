using ActiveControlApi.DTO.Manutencao;
using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.Services.Manutencao
{
    public interface IManutencaoService
    {
        // CRUD
        Task<ManutencaoDTO> Criar(CriarManutencaoDTO dto);
        Task<ManutencaoDTO> PegarPorId(int id);
        Task<IEnumerable<ManutencaoDTO>> PegarTodos();
        Task<ManutencaoDTO> Atualizar(int id, AtualizarManutencaoDTO dto);
        Task<bool> Remover(int id);

        // Paginação e Filtros
        Task<(IEnumerable<ManutencaoDTO> itens, int total)> PegarPaginado(int pagina, int tamanhoPagina, FiltroManutencaoDTO? filtro = null);
        Task<IEnumerable<ManutencaoDTO>> BuscarComFiltros(FiltroManutencaoDTO filtro);

        // Mudança de Status
        Task<ManutencaoDTO> IniciarManutencao(int id, int? usuarioResponsavelId = null);
        Task<ManutencaoDTO> ConcluirManutencao(int id, string? solucaoAplicada = null, decimal? custoReal = null);
        Task<ManutencaoDTO> CancelarManutencao(int id, string? motivo = null);

        // Consultas específicas
        Task<IEnumerable<ManutencaoDTO>> BuscarPorAtivo(int ativoId);
        Task<IEnumerable<ManutencaoDTO>> BuscarPorUsuarioResponsavel(int usuarioId);
        Task<IEnumerable<ManutencaoDTO>> BuscarAtrasadas();
        Task<IEnumerable<ManutencaoDTO>> BuscarPorPeriodo(DateTime dataInicio, DateTime dataFim);
    }
}
