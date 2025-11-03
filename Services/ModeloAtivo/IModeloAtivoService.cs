using ActiveControlApi.DTO.ModeloAtivo;

namespace ActiveControlApi.Services.ModeloAtivo
{
    public interface IModeloAtivoService
    {
        Task<IEnumerable<ModeloAtivoDTO>> PegarTodos();
        Task<ModeloAtivoDTO> PegarPorId(int id);
        Task<ModeloAtivoDTO> Criar(ModeloAtivoDTO dto);
        Task<ModeloAtivoDTO> Atualizar(int id, ModeloAtivoDTO dto);
        Task<bool> Remover(int id);
    }
}


