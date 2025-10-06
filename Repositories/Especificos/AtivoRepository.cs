using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class AtivoRepository : GenericRepository<Ativo>, IAtivoRepository
    {
        public AtivoRepository(AppDbContext context) : base(context)
        {
            

        }
    }
}
