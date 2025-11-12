using ActiveControlApi.DTO.UsuarioCargo;

namespace ActiveControlApi.Services.UsuarioCargo
{
    public interface IUsuarioCargoService
    {
        Task<IEnumerable<UsuarioCargoDTO>> PegarTodos();
        Task<UsuarioCargoDTO> PegarPorId(int id);
        Task<IEnumerable<UsuarioCargoDTO>> PegarPorUsuario(int usuarioId);
        Task<IEnumerable<UsuarioCargoDTO>> PegarPorCargo(int cargoId);
        Task<UsuarioCargoDTO> Atribuir(CriarUsuarioCargoDTO dto);
        Task<UsuarioCargoDTO> Atualizar(int id, AtualizarUsuarioCargoDTO dto);
        Task<UsuarioCargoDTO> Encerrar(int id);
        Task<bool> Remover(int id);
        Task VerificarAtribuicoesVencidas();
    }
}





