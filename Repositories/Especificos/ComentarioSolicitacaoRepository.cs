using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class ComentarioSolicitacaoRepository : GenericRepository<ComentarioSolicitacao>, IComentarioSolicitacaoRepository
    {
        public ComentarioSolicitacaoRepository(AppDbContext context) : base(context)
        {
        }
    }
}

