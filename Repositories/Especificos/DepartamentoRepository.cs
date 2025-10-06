using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class DepartamentoRepository : GenericRepository<Departamento> , IDepartamentoRepository
    {
        public DepartamentoRepository(AppDbContext context ) : base(context)
        {
            
        }
    }
}
