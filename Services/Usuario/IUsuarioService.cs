using ActiveControlApi.DTO.Usuario;

namespace ActiveControlApi.Services.Usuario
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDTO>> PegarTodos();
        Task<UsuarioDTO> PegarPorId(int id);
        Task<UsuarioDTO> Criar(UsuarioDTO dto);
        Task<UsuarioDTO> Atualizar(int id, UsuarioDTO dto);
        Task<bool> Remover(int id);

        bool CpfValido(string cpf);
        Task<bool> CpfJaExisteAsync(string cpf);
        Task<bool> EmailJaExisteAsync(string email);
    }
}





