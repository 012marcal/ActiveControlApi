using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class DevolucaoRepository : GenericRepository<Devolucao> , IDevolucaoRepository
    {
        public DevolucaoRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
