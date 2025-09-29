using ActiveControlApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }



    }
}
