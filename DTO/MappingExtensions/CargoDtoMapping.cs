using ActiveControlApi.DTO.Cargo;
using Model = ActiveControlApi.Models.Cargo;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class CargoDtoMapping
    {
        public static CargoDTO? ParaDto(this Model entity)
        {
            if (entity == null) return null;
            return new CargoDTO
            {
                Id = entity.Id,
                Descricao = entity.Descricao,
                CBO = entity.CBO
            };
        }

        public static Model? ParaEntity(this CargoDTO dto)
        {
            if (dto == null) return null;
            return new Model
            {
                Id = dto.Id,
                Descricao = dto.Descricao,
                CBO = dto.CBO
            };
        }

        public static Model? ParaEntity(this CriarCargoDTO dto)
        {
            if (dto == null) return null;
            return new Model
            {
                Descricao = dto.Descricao,
                CBO = dto.CBO
            };
        }

        public static IEnumerable<CargoDTO> ParaListaDto(this IEnumerable<Model> entities)
        {
            if (entities == null || !entities.Any()) return new List<CargoDTO>();
            return entities.Select(e => e.ParaDto()!).ToList();
        }
    }
}





