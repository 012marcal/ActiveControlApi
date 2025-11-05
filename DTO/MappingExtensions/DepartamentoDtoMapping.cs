using ActiveControlApi.DTO.Departamento;
using ModelDepartamento = ActiveControlApi.Models.Departamento;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class DepartamentoDtoMapping
    {
        public static DepartamentoDTO? ParaDepartamentoDto(this ModelDepartamento entity)
        {
            if (entity == null)
                return null;

            return new DepartamentoDTO
            {
                Id = entity.Id,
                Nome = entity.Nome,
                EmpresaId = entity.EmpresaId,
                EnderecoSetor = entity.EnderecoSetor,
                CidadeSetor = entity.CidadeSetor,
                UfSetor = entity.UfSetor,
                Cep = entity.Cep,
                LocalizacaoInterna = entity.LocalizacaoInterna
            };
        }

        public static ModelDepartamento? ParaDepartamento(this DepartamentoDTO dto)
        {
            if (dto == null)
                return null;

            return new ModelDepartamento
            {
                Id = dto.Id,
                Nome = dto.Nome,
                EmpresaId = dto.EmpresaId,
                EnderecoSetor = dto.EnderecoSetor ?? string.Empty,
                CidadeSetor = dto.CidadeSetor ?? string.Empty,
                UfSetor = dto.UfSetor ?? string.Empty,
                Cep = dto.Cep ?? string.Empty,
                LocalizacaoInterna = dto.LocalizacaoInterna ?? string.Empty
            };
        }

        public static IEnumerable<DepartamentoDTO> ParaListaDepartamentoDto(this IEnumerable<ModelDepartamento> entities)
        {
            if (entities == null || !entities.Any())
                return new List<DepartamentoDTO>();

            return entities.Select(e => e.ParaDepartamentoDto()!)
                           .ToList();
        }
    }
}





