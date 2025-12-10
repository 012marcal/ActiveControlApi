using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public interface IAtivoRepository : IGenericRepository<Ativo>
    {
        Task<Ativo?> GetAtivoCompletoAsync(int id);
        Task<IEnumerable<Ativo>> GetAllAtivoCompletoAsync();
    }
}
