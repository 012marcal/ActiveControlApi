using ActiveControlApi.Repositories.Especificos;

namespace ActiveControlApi.Repositories
{
    public interface IUnitOfWork
    {
        IAtivoRepository Ativo { get; }
        IAtivoDepartamentoRepository AtivoDepartamento { get; }
        IAtivoUsuarioRepository AtivoUsuario { get; }
        ICargoRepository Cargo { get; }
        ICategoriaAtivoRepository CategoriaAtivo { get; }
        IDepartamentoRepository Departamento { get; }
        IDevolucaoRepository Devolucao { get; }
        IEmpresaRepository Empresa { get; }
        IIncidenteRepository Incidente { get; }
        IManutencaoRepository Manutencao { get; }
        IModeloAtivoRepository ModeloAtivo { get; }   
        ISolicitacaoRepository Solicitacao { get; }
        IUsuarioRepository Usuario { get; }
        IUsuarioCargoRepository UsuarioCargo { get; }
        IUsuarioDepartamentoRepository UsuarioDepartamento { get; }
        IHistoricoMovimentacaoRepository HistoricoMovimentacao { get; }
        IComentarioSolicitacaoRepository ComentarioSolicitacao { get; }


        Task<int> CommitAsync();
    }
}
