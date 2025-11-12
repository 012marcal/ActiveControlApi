using ActiveControlApi.DTO.UsuarioDepartamento;

namespace ActiveControlApi.Services.UsuarioDepartamento
{
    public interface IUsuarioDepartamentoService
    {
        Task<IEnumerable<UsuarioDepartamentoDTO>> PegarTodos();
        Task<UsuarioDepartamentoDTO> PegarPorId(int id);
        Task<IEnumerable<UsuarioDepartamentoDTO>> PegarPorUsuario(int usuarioId);
        Task<IEnumerable<UsuarioDepartamentoDTO>> PegarPorDepartamento(int departamentoId);
        Task<IEnumerable<UsuarioDepartamentoDTO>> PegarPorEmpresa(int empresaId);
        Task<UsuarioDepartamentoDTO> Atribuir(CriarUsuarioDepartamentoDTO dto);
        Task<UsuarioDepartamentoDTO> Atualizar(int id, AtualizarUsuarioDepartamentoDTO dto);
        Task<UsuarioDepartamentoDTO> Encerrar(int id);
        Task<bool> Remover(int id);
        Task VerificarAtribuicoesVencidas();
    }
}





