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
using ActiveControlApi.Services.Solicitacao;
using ActiveControlApi.Services.Manutencao;
using ActiveControlApi.Services.Incidente;
using ActiveControlApi.Services.Devolucao;
using ActiveControlApi.Services.Auth;
using ActiveControlApi.Services.Historico;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar JWT
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer não configurado");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience não configurado");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Active Control API",
        Version = "v1",
        Description = "API para o sistema Active Control. Permite gerenciar ativos, departamentos e solicitações em tempo real."
    });

    c.EnableAnnotations();

    // Configurar Swagger para suportar JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
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
builder.Services.AddScoped<IHistoricoMovimentacaoRepository, HistoricoMovimentacaoRepository>();
builder.Services.AddScoped<IComentarioSolicitacaoRepository, ComentarioSolicitacaoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//SERVIOS >
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IAtivoService, AtivoService>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<ICategoriaAtivoService, CategoriaAtivoService>();
builder.Services.AddScoped<IModeloAtivoService, ModeloAtivoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAtivoUsuarioService, AtivoUsuarioService>();
builder.Services.AddScoped<IAtivoDepartamentoService, AtivoDepartamentoService>();
builder.Services.AddScoped<ISolicitacaoService, SolicitacaoService>();
builder.Services.AddScoped<IManutencaoService, ManutencaoService>();
builder.Services.AddScoped<IIncidenteService, IncidenteService>();
builder.Services.AddScoped<IDevolucaoService, DevolucaoService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHistoricoService, HistoricoService>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
