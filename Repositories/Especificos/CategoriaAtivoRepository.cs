using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class CategoriaAtivoRepository : GenericRepository<CategoriaAtivo>, ICategoriaAtivoRepository
    {
        public CategoriaAtivoRepository(AppDbContext context ) : base(context)
        {
            
        }
    }
}
