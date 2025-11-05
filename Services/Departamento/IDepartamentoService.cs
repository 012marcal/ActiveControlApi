using ActiveControlApi.DTO.Departamento;

namespace ActiveControlApi.Services.Departamento
{
    public interface IDepartamentoService
    {
        Task<IEnumerable<DepartamentoDTO>> PegarTodos();
        Task<DepartamentoDTO> PegarPorId(int id);
        Task<DepartamentoDTO> Criar(DepartamentoDTO dto);
        Task<DepartamentoDTO> Atualizar(int id, DepartamentoDTO dto);
        Task<bool> Remover(int id);
    }
}





