using ActiveControlApi.DTO.ModeloAtivo;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;

namespace ActiveControlApi.Services.ModeloAtivo
{
    public class ModeloAtivoService : IModeloAtivoService
    {
        private readonly IUnitOfWork _uow;

        public ModeloAtivoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<ModeloAtivoDTO>> PegarTodos()
        {
            var list = await _uow.ModeloAtivo.GetAll();
            if (!list.Any()) return Enumerable.Empty<ModeloAtivoDTO>();
            return list.ParaListaModeloDto();
        }

        public async Task<ModeloAtivoDTO> PegarPorId(int id)
        {
            var entity = await _uow.ModeloAtivo.Get(m => m.Id == id);
            if (entity == null) throw new KeyNotFoundException($"ModeloAtivo com id {id} não encontrado.");
            return entity.ParaModeloDto();
        }

        public async Task<ModeloAtivoDTO> Criar(ModeloAtivoDTO dto)
        {
            var entity = dto.ParaModelo();
            var created = _uow.ModeloAtivo.Create(entity);
            await _uow.CommitAsync();
            return created.ParaModeloDto();
        }

        public async Task<ModeloAtivoDTO> Atualizar(int id, ModeloAtivoDTO dto)
        {
            var entity = await _uow.ModeloAtivo.Get(m => m.Id == id);
            if (entity == null) throw new KeyNotFoundException($"ModeloAtivo com id {id} não encontrado.");
            if (!string.IsNullOrWhiteSpace(dto.Nome)) entity.Nome = dto.Nome;
            if (!string.IsNullOrWhiteSpace(dto.Fabricante)) entity.Fabricante = dto.Fabricante;
            if (!string.IsNullOrWhiteSpace(dto.Especificacoes)) entity.Especificacoes = dto.Especificacoes;
            _uow.ModeloAtivo.Update(entity);
            await _uow.CommitAsync();
            return entity.ParaModeloDto();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.ModeloAtivo.Get(m => m.Id == id);
            if (entity == null) throw new KeyNotFoundException($"ModeloAtivo com id {id} não encontrado.");

            // Regra: não remover se houver Ativo vinculado
            var existeAtivo = await _uow.Ativo.Any(a => a.ModeloAtivoId == id);
            if (existeAtivo) throw new InvalidOperationException("Não é possível remover Modelo com Ativos vinculados.");

            _uow.ModeloAtivo.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}





