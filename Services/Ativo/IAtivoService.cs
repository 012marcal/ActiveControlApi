using ActiveControlApi.DTO.Ativo;

namespace ActiveControlApi.Services.Ativo
{
    public interface IAtivoService
    {

        Task<IEnumerable<AtivoDTO>> PegarTodos();

        Task<AtivoDTO> PegarPorId(int idAtivo);

        Task<AtivoDTO> CriarAtivo(AtivoDTO ativoRegistro);

        Task<AtivoDTO> AtualizarAtivo(int idAtivo, AtivoDTO ativoRegistro);

        Task<bool> RemoverAtivo(int idAtivo);


    }
}
