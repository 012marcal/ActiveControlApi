using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class AtivoUsuarioRepository : GenericRepository<AtivoUsuario>, IAtivoUsuarioRepository
    {
        public AtivoUsuarioRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
