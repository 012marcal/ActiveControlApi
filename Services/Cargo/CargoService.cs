using ActiveControlApi.DTO.Cargo;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;

namespace ActiveControlApi.Services.Cargo
{
    public class CargoService : ICargoService
    {
        private readonly IUnitOfWork _uow;

        public CargoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<CargoDTO>> PegarTodos()
        {
            var list = await _uow.Cargo.GetAll();
            if (!list.Any()) return Enumerable.Empty<CargoDTO>();
            return list.ParaListaDto();
        }

        public async Task<CargoDTO> PegarPorId(int id)
        {
            var entity = await _uow.Cargo.Get(c => c.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Cargo {id} não encontrado.");
            return entity.ParaDto()!;
        }

        public async Task<CargoDTO> Criar(CriarCargoDTO dto)
        {
            // Verificar se já existe cargo com mesmo CBO
            var existeCBO = await _uow.Cargo.Any(c => c.CBO == dto.CBO);
            if (existeCBO)
                throw new InvalidOperationException($"Já existe um cargo cadastrado com o CBO {dto.CBO}.");

            var entity = dto.ParaEntity();
            var created = _uow.Cargo.Create(entity);
            await _uow.CommitAsync();
            return created.ParaDto()!;
        }

        public async Task<CargoDTO> Atualizar(int id, AtualizarCargoDTO dto)
        {
            var entity = await _uow.Cargo.Get(c => c.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Cargo {id} não encontrado.");

            // Verificar se CBO está sendo alterado e se já existe outro cargo com esse CBO
            if (entity.CBO != dto.CBO)
            {
                var existeCBO = await _uow.Cargo.Any(c => c.CBO == dto.CBO && c.Id != id);
                if (existeCBO)
                    throw new InvalidOperationException($"Já existe outro cargo cadastrado com o CBO {dto.CBO}.");
            }

            entity.Descricao = dto.Descricao;
            entity.CBO = dto.CBO;

            _uow.Cargo.Update(entity);
            await _uow.CommitAsync();
            return entity.ParaDto()!;
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.Cargo.Get(c => c.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Cargo {id} não encontrado.");

            // Verificar se há usuários vinculados a este cargo
            var temVinculos = await _uow.UsuarioCargo.Any(uc => uc.CargoId == id);
            if (temVinculos)
                throw new InvalidOperationException("Não é possível remover cargo que possui usuários vinculados.");

            _uow.Cargo.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}





