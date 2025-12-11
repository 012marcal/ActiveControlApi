using ActiveControlApi.DTO.AtivoDepartamento;

namespace ActiveControlApi.Services.AtivoDepartamento
{
    public interface IAtivoDepartamentoService
    {
        Task<IEnumerable<AtivoDepartamentoDTO>> PegarTodos();
        Task<AtivoDepartamentoDTO> PegarPorId(int id);
        Task<AtivoDepartamentoDTO> Alocar(AtivoDepartamentoDTO dto);
        Task<AtivoDepartamentoDTO> Atualizar(int id, AtivoDepartamentoDTO dto);
        Task<AtivoDepartamentoDTO> Encerrar(int ativoId, DateTime dataFim);
        Task<bool> Remover(int id);
        Task VerificarAlocacoesVencidas();
    }
}


