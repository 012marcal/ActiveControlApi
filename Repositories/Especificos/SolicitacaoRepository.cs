using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class SolicitacaoRepository : GenericRepository<Solicitacao>, ISolicitacaoRepository
    {

        public SolicitacaoRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
