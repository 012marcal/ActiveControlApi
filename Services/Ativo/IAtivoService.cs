using ActiveControlApi.DTO.Ativo;
using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.Services.Ativo
{
    public interface IAtivoService
    {
        Task<IEnumerable<AtivoDTO>> PegarTodos();
        Task<AtivoDTO> PegarPorId(int idAtivo);
        Task<AtivoDTO> CriarAtivo(AtivoDTO ativoRegistro);
        Task<AtivoDTO> AtualizarAtivo(int idAtivo, AtivoDTO ativoRegistro);
        Task<bool> RemoverAtivo(int idAtivo);

        // Consultas avançadas
        Task<IEnumerable<AtivoDTO>> BuscarPorStatus(statusAtivo status);
        Task<IEnumerable<AtivoDTO>> BuscarPorCategoria(int categoriaId);
        Task<IEnumerable<AtivoDTO>> BuscarPorModelo(int modeloId);
        Task<IEnumerable<AtivoDTO>> BuscarDisponiveis();
        Task<IEnumerable<AtivoDTO>> BuscarEmManutencao();
        Task<AtivoDTO> BuscarPorNumeroPatrimonio(string numPatrimonio);
        Task<AtivoDTO> BuscarPorNumeroSerie(string numSerie);

        // Depreciação
        Task<decimal> CalcularDepreciacao(int idAtivo);
        Task<IEnumerable<AtivoDTO>> ListarProximosVencimento(int mesesAntecedencia = 6);

        // Relatórios
        Task<Dictionary<string, object>> ObterEstatisticas();
        Task<IEnumerable<AtivoDTO>> BuscarPorUsuario(int usuarioId);
        Task<IEnumerable<AtivoDTO>> BuscarPorDepartamento(int departamentoId);

        // Paginação
        Task<(IEnumerable<AtivoDTO> itens, int total)> PegarPaginado(int pagina, int tamanhoPagina);
    }
}
