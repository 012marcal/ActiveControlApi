using ActiveControlApi.DTO.AtivoUsuario;

namespace ActiveControlApi.Services.AtivoUsuario
{
    public interface IAtivoUsuarioService
    {
        Task<IEnumerable<AtivoUsuarioDTO>> PegarTodos();
        Task<AtivoUsuarioDTO> PegarPorId(int id);
        Task<AtivoUsuarioDTO> Alocar(AtivoUsuarioDTO dto);
        Task<AtivoUsuarioDTO> Atualizar(int id, AtivoUsuarioDTO dto);
        Task<AtivoUsuarioDTO> Encerrar(int ativoId, DateTime dataFim);
        Task<bool> Remover(int id);
        Task VerificarAlocacoesVencidas();
    }
}


