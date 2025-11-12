using ActiveControlApi.DTO.Cargo;

namespace ActiveControlApi.Services.Cargo
{
    public interface ICargoService
    {
        Task<IEnumerable<CargoDTO>> PegarTodos();
        Task<CargoDTO> PegarPorId(int id);
        Task<CargoDTO> Criar(CriarCargoDTO dto);
        Task<CargoDTO> Atualizar(int id, AtualizarCargoDTO dto);
        Task<bool> Remover(int id);
    }
}





