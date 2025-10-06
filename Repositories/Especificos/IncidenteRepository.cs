using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class IncidenteRepository : GenericRepository<Incidente>, IIncidenteRepository
    {
        public IncidenteRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
