using ActiveControlApi.DTO.AtivoUsuario;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Services.AtivoUsuario
{
    public class AtivoUsuarioService : IAtivoUsuarioService
    {
        private readonly IUnitOfWork _uow;

        public AtivoUsuarioService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<AtivoUsuarioDTO>> PegarTodos()
        {
            var list = await _uow.AtivoUsuario.GetAll();
            if (!list.Any()) return Enumerable.Empty<AtivoUsuarioDTO>();
            return list.ParaListaDto();
        }

        public async Task<AtivoUsuarioDTO> PegarPorId(int id)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");
            return entity.ParaDto();
        }

        public async Task<AtivoUsuarioDTO> Alocar(AtivoUsuarioDTO dto)
        {
            // regra: não permitir alocação se já existe alocação aberta do ativo
            bool abertoUsuario = await _uow.AtivoUsuario.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            bool abertoDepartamento = await _uow.AtivoDepartamento.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            if (abertoUsuario || abertoDepartamento)
                throw new InvalidOperationException("Ativo já está alocado (usuário ou departamento).");

            var entity = dto.ParaEntity();
            entity.DataInicio = entity.DataInicio == default ? DateTime.UtcNow : entity.DataInicio;
            var created = _uow.AtivoUsuario.Create(entity);
            await _uow.CommitAsync();
            return created.ParaDto();
        }

        public async Task<AtivoUsuarioDTO> Encerrar(int id)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");
            if (entity.DataFim != null) throw new InvalidOperationException("Vínculo já encerrado.");
            entity.DataFim = DateTime.UtcNow;
            _uow.AtivoUsuario.Update(entity);
            await _uow.CommitAsync();
            return entity.ParaDto();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");
            _uow.AtivoUsuario.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}


