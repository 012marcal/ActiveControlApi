using ActiveControlApi.DTO.Devolucao;

namespace ActiveControlApi.Services.Devolucao
{
    public interface IDevolucaoService
    {
        Task<IEnumerable<DevolucaoDTO>> PegarTodos();
        Task<DevolucaoDTO> PegarPorId(int idDevolucao);
        Task<DevolucaoDTO> CriarDevolucao(DevolucaoDTO devolucaoRegistro);
        Task<DevolucaoDTO> AtualizarDevolucao(int idDevolucao, DevolucaoDTO devolucaoRegistro);
        Task<bool> RemoverDevolucao(int idDevolucao);
    }
}



