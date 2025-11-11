using ActiveControlApi.Data;
using ActiveControlApi.Models;

namespace ActiveControlApi.Repositories.Especificos
{
    public class HistoricoMovimentacaoRepository : GenericRepository<HistoricoMovimentacao>, IHistoricoMovimentacaoRepository
    {
        public HistoricoMovimentacaoRepository(AppDbContext context) : base(context)
        {
        }
    }
}


