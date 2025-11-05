using ActiveControlApi.DTO.CategoriaAtivo;
using ModelCategoria = ActiveControlApi.Models.CategoriaAtivo;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class CategoriaAtivoDtoMapping
    {
        public static CategoriaAtivoDTO? ParaCategoriaDto(this ModelCategoria entity)
        {
            if (entity == null) return null;
            return new CategoriaAtivoDTO
            {
                Id = entity.Id,
                NomeCategoria = entity.NomeCategoria
            };
        }

        public static ModelCategoria? ParaCategoria(this CategoriaAtivoDTO dto)
        {
            if (dto == null) return null;
            return new ModelCategoria
            {
                Id = dto.Id,
                NomeCategoria = dto.NomeCategoria
            };
        }

        public static IEnumerable<CategoriaAtivoDTO> ParaListaCategoriaDto(this IEnumerable<ModelCategoria> entities)
        {
            if (entities == null || !entities.Any()) return new List<CategoriaAtivoDTO>();
            return entities.Select(e => e.ParaCategoriaDto()!).ToList();
        }
    }
}





