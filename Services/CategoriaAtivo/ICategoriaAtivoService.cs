using ActiveControlApi.DTO.CategoriaAtivo;

namespace ActiveControlApi.Services.CategoriaAtivo
{
    public interface ICategoriaAtivoService
    {
        Task<IEnumerable<CategoriaAtivoDTO>> PegarTodas();
        Task<CategoriaAtivoDTO> PegarPorId(int id);
        Task<CategoriaAtivoDTO> Criar(CategoriaAtivoDTO dto);
        Task<CategoriaAtivoDTO> Atualizar(int id, CategoriaAtivoDTO dto);
        Task<bool> Remover(int id);
    }
}





