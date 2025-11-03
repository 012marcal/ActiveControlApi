using ActiveControlApi.DTO.AtivoDepartamento;
using Model = ActiveControlApi.Models.AtivoDepartamento;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class AtivoDepartamentoDtoMapping
    {
        public static AtivoDepartamentoDTO? ParaDto(this Model entity)
        {
            if (entity == null) return null;
            return new AtivoDepartamentoDTO
            {
                Id = entity.Id,
                AtivoId = entity.AtivoId,
                DepartamentoId = entity.DepartamentoId,
                DataInicio = entity.DataInicio,
                DataFim = entity.DataFim
            };
        }

        public static Model? ParaEntity(this AtivoDepartamentoDTO dto)
        {
            if (dto == null) return null;
            return new Model
            {
                Id = dto.Id,
                AtivoId = dto.AtivoId,
                DepartamentoId = dto.DepartamentoId,
                DataInicio = dto.DataInicio ?? DateTime.UtcNow,
                DataFim = dto.DataFim
            };
        }

        public static IEnumerable<AtivoDepartamentoDTO> ParaListaDto(this IEnumerable<Model> entities)
        {
            if (entities == null || !entities.Any()) return new List<AtivoDepartamentoDTO>();
            return entities.Select(e => e.ParaDto()!).ToList();
        }
    }
}


