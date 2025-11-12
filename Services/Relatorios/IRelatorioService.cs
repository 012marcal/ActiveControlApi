using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.Services.Relatorios
{
    public interface IRelatorioService
    {
        Task<object> QuantidadeAtivos(string groupBy); // categoria|departamento
        Task<IEnumerable<object>> AtivosPorProfissional();
        Task<IEnumerable<object>> UsuariosAtivos();
        Task<IEnumerable<object>> CustoPorAtivo();
        Task<IEnumerable<object>> ManutencaoPreventiva();
    }
}









