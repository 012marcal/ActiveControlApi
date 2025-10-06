using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class AtivoDepartamentoRepository : GenericRepository<AtivoDepartamento> ,IAtivoDepartamentoRepository
    {
        public AtivoDepartamentoRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
