using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class ManutencaoRepository : GenericRepository<Manutencao>, IManutencaoRepository
    {
        public ManutencaoRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
