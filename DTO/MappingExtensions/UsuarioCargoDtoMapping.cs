using ActiveControlApi.DTO.UsuarioCargo;
using Model = ActiveControlApi.Models.UsuarioCargo;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class UsuarioCargoDtoMapping
    {
        public static UsuarioCargoDTO? ParaDto(this Model entity)
        {
            if (entity == null) return null;
            return new UsuarioCargoDTO
            {
                Id = entity.Id,
                UsuarioId = entity.UsuarioId,
                UsuarioNome = entity.Usuario?.NomeCompleto,
                CargoId = entity.CargoId,
                CargoDescricao = entity.Cargo?.Descricao,
                DataInicio = entity.DataInicio,
                DataFim = entity.DataFim
            };
        }

        public static Model? ParaEntity(this CriarUsuarioCargoDTO dto)
        {
            if (dto == null) return null;
            return new Model
            {
                UsuarioId = dto.UsuarioId,
                CargoId = dto.CargoId,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim
            };
        }

        public static IEnumerable<UsuarioCargoDTO> ParaListaDto(this IEnumerable<Model> entities)
        {
            if (entities == null || !entities.Any()) return new List<UsuarioCargoDTO>();
            return entities.Select(e => e.ParaDto()!).ToList();
        }
    }
}





