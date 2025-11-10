using ActiveControlApi.DTO.ModeloAtivo;
using ModelModelo = ActiveControlApi.Models.ModeloAtivo;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class ModeloAtivoDtoMapping
    {
        public static ModeloAtivoDTO? ParaModeloDto(this ModelModelo entity)
        {
            if (entity == null) return null;
            return new ModeloAtivoDTO
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Fabricante = entity.Fabricante,
                Especificacoes = entity.Especificacoes
            };
        }

        public static ModelModelo? ParaModelo(this ModeloAtivoDTO dto)
        {
            if (dto == null) return null;
            return new ModelModelo
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Fabricante = dto.Fabricante,
                Especificacoes = dto.Especificacoes
            };
        }

        public static IEnumerable<ModeloAtivoDTO> ParaListaModeloDto(this IEnumerable<ModelModelo> entities)
        {
            if (entities == null || !entities.Any()) return new List<ModeloAtivoDTO>();
            return entities.Select(e => e.ParaModeloDto()!).ToList();
        }
    }
}


