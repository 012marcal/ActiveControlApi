using ActiveControlApi.DTO.AtivoDepartamento;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;

namespace ActiveControlApi.Services.AtivoDepartamento
{
    public class AtivoDepartamentoService : IAtivoDepartamentoService
    {
        private readonly IUnitOfWork _uow;

        public AtivoDepartamentoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<AtivoDepartamentoDTO>> PegarTodos()
        {
            var list = await _uow.AtivoDepartamento.GetAll();
            if (!list.Any()) return Enumerable.Empty<AtivoDepartamentoDTO>();
            return list.ParaListaDto();
        }

        public async Task<AtivoDepartamentoDTO> PegarPorId(int id)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");
            return entity.ParaDto();
        }

        public async Task<AtivoDepartamentoDTO> Alocar(AtivoDepartamentoDTO dto)
        {
            bool abertoUsuario = await _uow.AtivoUsuario.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            bool abertoDepartamento = await _uow.AtivoDepartamento.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            if (abertoUsuario || abertoDepartamento)
                throw new InvalidOperationException("Ativo já está alocado (usuário ou departamento).");

            var entity = dto.ParaEntity();
            entity.DataInicio = entity.DataInicio == default ? DateTime.UtcNow : entity.DataInicio;
            var created = _uow.AtivoDepartamento.Create(entity);
            await _uow.CommitAsync();
            return created.ParaDto();
        }

        public async Task<AtivoDepartamentoDTO> Encerrar(int id)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");
            if (entity.DataFim != null) throw new InvalidOperationException("Vínculo já encerrado.");
            entity.DataFim = DateTime.UtcNow;
            _uow.AtivoDepartamento.Update(entity);
            await _uow.CommitAsync();
            return entity.ParaDto();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");
            _uow.AtivoDepartamento.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}





