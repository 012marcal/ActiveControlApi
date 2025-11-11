using ActiveControlApi.DTO.Devolucao;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;

namespace ActiveControlApi.Services.Devolucao
{
    public class DevolucaoService : IDevolucaoService
    {
        private readonly IUnitOfWork _uow;

        public DevolucaoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<DevolucaoDTO>> PegarTodos()
        {
            var devolucoes = await _uow.Devolucao.GetAll();
            if (!devolucoes.Any())
                return Enumerable.Empty<DevolucaoDTO>();

            return devolucoes.ParaListaDevolucaoDto();
        }

        public async Task<DevolucaoDTO> PegarPorId(int idDevolucao)
        {
            var devolucao = await _uow.Devolucao.Get(d => d.Id == idDevolucao);
            if (devolucao == null)
                throw new KeyNotFoundException($"Devolução com id {idDevolucao} não encontrada.");

            return devolucao.ParaDevolucaoDto() ?? throw new InvalidOperationException("Erro ao converter devolução.");
        }

        public async Task<DevolucaoDTO> CriarDevolucao(DevolucaoDTO devolucaoRegistro)
        {
            var entity = devolucaoRegistro.ParaDevolucao();
            if (entity == null)
                throw new ArgumentNullException(nameof(devolucaoRegistro), "Não foi possível criar a devolução.");

            var criado = _uow.Devolucao.Create(entity);
            await _uow.CommitAsync();

            return criado.ParaDevolucaoDto() ?? throw new InvalidOperationException("Erro ao criar devolução.");
        }

        public async Task<DevolucaoDTO> AtualizarDevolucao(int idDevolucao, DevolucaoDTO devolucaoRegistro)
        {
            var entity = await _uow.Devolucao.Get(d => d.Id == idDevolucao);
            if (entity == null)
                throw new KeyNotFoundException($"Devolução com id {idDevolucao} não encontrada.");

            if (devolucaoRegistro.SolicitacaoId > 0)
                entity.SolicitacaoId = devolucaoRegistro.SolicitacaoId;
            if (!string.IsNullOrWhiteSpace(devolucaoRegistro.MotivoDevolucao))
                entity.MotivoDevolucao = devolucaoRegistro.MotivoDevolucao;

            _uow.Devolucao.Update(entity);
            await _uow.CommitAsync();

            return entity.ParaDevolucaoDto() ?? throw new InvalidOperationException("Erro ao atualizar devolução.");
        }

        public async Task<bool> RemoverDevolucao(int idDevolucao)
        {
            var entity = await _uow.Devolucao.Get(d => d.Id == idDevolucao);
            if (entity == null)
                throw new KeyNotFoundException($"Devolução com id {idDevolucao} não encontrada.");

            _uow.Devolucao.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}

