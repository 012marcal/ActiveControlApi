using ActiveControlApi.DTO.Departamento;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;

namespace ActiveControlApi.Services.Departamento
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IUnitOfWork _uow;

        public DepartamentoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<DepartamentoDTO>> PegarTodos()
        {
            var list = await _uow.Departamento.GetAll();
            if (!list.Any()) return Enumerable.Empty<DepartamentoDTO>();
            return list.ParaListaDepartamentoDto();
        }

        public async Task<DepartamentoDTO> PegarPorId(int id)
        {
            var entity = await _uow.Departamento.Get(d => d.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Departamento com id {id} não encontrado.");
            return entity.ParaDepartamentoDto();
        }

        public async Task<DepartamentoDTO> Criar(DepartamentoDTO dto)
        {
            var entity = dto.ParaDepartamento();
            var created = _uow.Departamento.Create(entity);
            await _uow.CommitAsync();
            return created.ParaDepartamentoDto();
        }

        public async Task<DepartamentoDTO> Atualizar(int id, DepartamentoDTO dto)
        {
            var entity = await _uow.Departamento.Get(d => d.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Departamento com id {id} não encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.Nome)) entity.Nome = dto.Nome;
            if (dto.EmpresaId > 0) entity.EmpresaId = dto.EmpresaId;
            if (!string.IsNullOrWhiteSpace(dto.EnderecoSetor)) entity.EnderecoSetor = dto.EnderecoSetor;
            if (!string.IsNullOrWhiteSpace(dto.CidadeSetor)) entity.CidadeSetor = dto.CidadeSetor;
            if (!string.IsNullOrWhiteSpace(dto.UfSetor)) entity.UfSetor = dto.UfSetor;
            if (!string.IsNullOrWhiteSpace(dto.Cep)) entity.Cep = dto.Cep;
            if (!string.IsNullOrWhiteSpace(dto.LocalizacaoInterna)) entity.LocalizacaoInterna = dto.LocalizacaoInterna;

            _uow.Departamento.Update(entity);
            await _uow.CommitAsync();
            return entity.ParaDepartamentoDto();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.Departamento.Get(d => d.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Departamento com id {id} não encontrado.");
            _uow.Departamento.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}


