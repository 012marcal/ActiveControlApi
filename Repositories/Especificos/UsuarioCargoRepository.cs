using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class UsuarioCargoRepository : GenericRepository<UsuarioCargo>, IUsuarioCargoRepository
    {
        public UsuarioCargoRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
