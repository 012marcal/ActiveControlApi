using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class ModeloAtivoRepository : GenericRepository<ModeloAtivo>, IModeloAtivoRepository
    {
        public ModeloAtivoRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
