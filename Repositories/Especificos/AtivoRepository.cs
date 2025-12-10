using ActiveControlApi.Data;
using ActiveControlApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Repositories.Especificos
{
    public class AtivoRepository : GenericRepository<Ativo>, IAtivoRepository
    {
        public AtivoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Ativo?> GetAtivoCompletoAsync(int id)
        {
            return await _context.Ativo
                .Include(a => a.ModeloAtivo)
                .Include(a => a.CategoriaAtivo)
                .Include(a => a.AtivoUsuario.Where(x => x.DataFim == null))
                    .ThenInclude(x => x.Usuario)
                .Include(a => a.AtivoDepartamento.Where(x => x.DataFim == null))
                    .ThenInclude(x => x.Departamento)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Ativo>> GetAllAtivoCompletoAsync()
        {
            return await _context.Ativo
                .Include(a => a.ModeloAtivo)
                .Include(a => a.CategoriaAtivo)
                .Include(a => a.AtivoUsuario.Where(x => x.DataFim == null))
                    .ThenInclude(x => x.Usuario)
                .Include(a => a.AtivoDepartamento.Where(x => x.DataFim == null))
                    .ThenInclude(x => x.Departamento)
                .ToListAsync();
        }

    }
}
