using ActiveControlApi.Data;
using ActiveControlApi.Repositories.Especificos;

namespace ActiveControlApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        private IAtivoRepository _ativo;
        private IAtivoDepartamentoRepository _ativoDepartamento;
        private IAtivoUsuarioRepository _ativoUsuario;
        private ICargoRepository _cargo;
        private ICategoriaAtivoRepository _categoriaAtivo;
        private IDepartamentoRepository _departamento;
        private IDevolucaoRepository _devolucao;
        private IEmpresaRepository _empresa;
        private IIncidenteRepository _incidente;
        private IManutencaoRepository _manutencao;
        private IModeloAtivoRepository _modeloAtivo;
        private ISolicitacaoRepository _solicitacao;
        private IUsuarioRepository _usuario;
        private IUsuarioCargoRepository _usuarioCargo;
        private IUsuarioDepartamentoRepository _usuarioDepartamento;

        public IAtivoRepository Ativo
            => _ativo ??= new AtivoRepository(_context);

        public IAtivoDepartamentoRepository AtivoDepartamento
            => _ativoDepartamento ??= new AtivoDepartamentoRepository(_context);

        public IAtivoUsuarioRepository AtivoUsuario
            => _ativoUsuario ??= new AtivoUsuarioRepository(_context);

        public ICargoRepository Cargo 
            => _cargo ??= new CargoRepository(_context);

        public ICategoriaAtivoRepository CategoriaAtivo 
            => _categoriaAtivo ??= new CategoriaAtivoRepository(_context);

        public IDepartamentoRepository Departamento
            => _departamento ??= new DepartamentoRepository(_context);

        public IDevolucaoRepository Devolucao 
            => _devolucao ??= new DevolucaoRepository(_context);

        public IEmpresaRepository Empresa
            => _empresa ??= new EmpresaRepository(_context);

        public IIncidenteRepository Incidente 
            => _incidente ??= new IncidenteRepository(_context);

        public IManutencaoRepository Manutencao 
            => _manutencao ??= new ManutencaoRepository(_context);

        public IModeloAtivoRepository ModeloAtivo
            => _modeloAtivo ??= new ModeloAtivoRepository(_context);

        public ISolicitacaoRepository Solicitacao
            => _solicitacao ??= new SolicitacaoRepository(_context);

        public IUsuarioRepository Usuario
            => _usuario ??= new UsuarioRepository(_context);

        public IUsuarioCargoRepository UsuarioCargo
            => _usuarioCargo ??= new UsuarioCargoRepository(_context);

        public IUsuarioDepartamentoRepository UsuarioDepartamento
            => _usuarioDepartamento ??= new UsuarioDepartamentoRepository(_context);
        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

    }
}
