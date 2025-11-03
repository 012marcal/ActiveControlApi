using ActiveControlApi.Data;
using ActiveControlApi.Repositories;
using ActiveControlApi.Repositories.Especificos;
using ActiveControlApi.Services.Empresa;
using ActiveControlApi.Services.Ativo;
using ActiveControlApi.Services.Departamento;
using ActiveControlApi.Services.CategoriaAtivo;
using ActiveControlApi.Services.ModeloAtivo;
using ActiveControlApi.Services.Usuario;
using ActiveControlApi.Services.AtivoUsuario;
using ActiveControlApi.Services.AtivoDepartamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Necess�rio para OpenApiInfo
using Swashbuckle.AspNetCore.Annotations; // Adicionar este using para EnableAnnotations()

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Active Control API",
        Version = "v1",
        Description = "API para o sistema Active Control. Permite gerenciar ativos, departamentos e solicita��es em tempo real."
    });

    
    c.EnableAnnotations(); 
});


var connectionString = builder.Configuration.GetConnectionString("DefaultString");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});


//REPOSITORIES >
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAtivoRepository, AtivoRepository>();
builder.Services.AddScoped<IAtivoDepartamentoRepository, AtivoDepartamentoRepository>();
builder.Services.AddScoped<IAtivoUsuarioRepository, AtivoUsuarioRepository>();
builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<ICategoriaAtivoRepository, CategoriaAtivoRepository>();
builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
builder.Services.AddScoped<IDevolucaoRepository, DevolucaoRepository>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IIncidenteRepository, IncidenteRepository>();
builder.Services.AddScoped<IManutencaoRepository, ManutencaoRepository>();
builder.Services.AddScoped<IModeloAtivoRepository, ModeloAtivoRepository>();
builder.Services.AddScoped<ISolicitacaoRepository, SolicitacaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioCargoRepository, UsuarioCargoRepository>();
builder.Services.AddScoped<IUsuarioDepartamentoRepository, UsuarioDepartamentoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//SERVI�OS >
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IAtivoService, AtivoService>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<ICategoriaAtivoService, CategoriaAtivoService>();
builder.Services.AddScoped<IModeloAtivoService, ModeloAtivoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAtivoUsuarioService, AtivoUsuarioService>();
builder.Services.AddScoped<IAtivoDepartamentoService, AtivoDepartamentoService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Active Control API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
