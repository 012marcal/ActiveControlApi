using ActiveControlApi.DTO.AtivoUsuario;
using Model = ActiveControlApi.Models.AtivoUsuario;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class AtivoUsuarioDtoMapping
    {
        public static AtivoUsuarioDTO? ParaDto(this Model entity)
        {
            if (entity == null) return null;
            return new AtivoUsuarioDTO
            {
                Id = entity.Id,
                AtivoId = entity.AtivoId,
                UsuarioId = entity.UsuarioId,
                DataInicio = entity.DataInicio,
                DataFim = entity.DataFim
            };
        }

        public static Model? ParaEntity(this AtivoUsuarioDTO dto)
        {
            if (dto == null) return null;
            return new Model
            {
                Id = dto.Id,
                AtivoId = dto.AtivoId,
                UsuarioId = dto.UsuarioId,
                DataInicio = dto.DataInicio ?? DateTime.UtcNow,
                DataFim = dto.DataFim
            };
        }

        public static IEnumerable<AtivoUsuarioDTO> ParaListaDto(this IEnumerable<Model> entities)
        {
            if (entities == null || !entities.Any()) return new List<AtivoUsuarioDTO>();
            return entities.Select(e => e.ParaDto()!).ToList();
        }
    }
}





