using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class UsuarioDepartamentoRepository : GenericRepository<UsuarioDepartamento>, IUsuarioDepartamentoRepository
    {
        public UsuarioDepartamentoRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
