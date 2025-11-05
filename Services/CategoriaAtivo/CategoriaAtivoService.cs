using ActiveControlApi.DTO.CategoriaAtivo;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;

namespace ActiveControlApi.Services.CategoriaAtivo
{
    public class CategoriaAtivoService : ICategoriaAtivoService
    {
        private readonly IUnitOfWork _uow;

        public CategoriaAtivoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<CategoriaAtivoDTO>> PegarTodas()
        {
            var list = await _uow.CategoriaAtivo.GetAll();
            if (!list.Any()) return Enumerable.Empty<CategoriaAtivoDTO>();
            return list.ParaListaCategoriaDto();
        }

        public async Task<CategoriaAtivoDTO> PegarPorId(int id)
        {
            var entity = await _uow.CategoriaAtivo.Get(c => c.Id == id);
            if (entity == null) throw new KeyNotFoundException($"CategoriaAtivo com id {id} não encontrada.");
            return entity.ParaCategoriaDto();
        }

        public async Task<CategoriaAtivoDTO> Criar(CategoriaAtivoDTO dto)
        {
            var entity = dto.ParaCategoria();
            var created = _uow.CategoriaAtivo.Create(entity);
            await _uow.CommitAsync();
            return created.ParaCategoriaDto();
        }

        public async Task<CategoriaAtivoDTO> Atualizar(int id, CategoriaAtivoDTO dto)
        {
            var entity = await _uow.CategoriaAtivo.Get(c => c.Id == id);
            if (entity == null) throw new KeyNotFoundException($"CategoriaAtivo com id {id} não encontrada.");
            if (!string.IsNullOrWhiteSpace(dto.NomeCategoria)) entity.NomeCategoria = dto.NomeCategoria;
            _uow.CategoriaAtivo.Update(entity);
            await _uow.CommitAsync();
            return entity.ParaCategoriaDto();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.CategoriaAtivo.Get(c => c.Id == id);
            if (entity == null) throw new KeyNotFoundException($"CategoriaAtivo com id {id} não encontrada.");

            // Regra: não remover se houver Ativo vinculado
            var existeAtivo = await _uow.Ativo.Any(a => a.CategoriaAtivoId == id);
            if (existeAtivo) throw new InvalidOperationException("Não é possível remover Categoria com Ativos vinculados.");

            _uow.CategoriaAtivo.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}





