using ActiveControlApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ActiveControlApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Ativo> Ativo { get; set; }
        public DbSet<AtivoDepartamento> AtivoDepartamento { get; set; }
        public DbSet<AtivoUsuario> AtivoUsuario { get; set; }
        public DbSet<Cargo> Cargo {  get; set; }
        public DbSet<CategoriaAtivo> CategoriaAtivo { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Devolucao> Devolucao { get; set; }
        public DbSet<Empresa> Empresa   { get; set; }
        public DbSet<Incidente> Incidente {  get; set; }
        public DbSet<Manutencao> Manutencao { get; set; }
        public DbSet<ModeloAtivo> ModeloAtivo { get; set; }
        public DbSet<Solicitacao> Solicitacao { get; set; }
        public DbSet<Usuario> Usuario   { get; set; }
        public DbSet<UsuarioCargo> UsuarioCargo { get; set; }
        public DbSet<UsuarioDepartamento> UsuarioDepartamento { get; set; }




    }
}
