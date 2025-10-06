using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class CargoRepository : GenericRepository<Cargo>, ICargoRepository
    {
        public CargoRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
