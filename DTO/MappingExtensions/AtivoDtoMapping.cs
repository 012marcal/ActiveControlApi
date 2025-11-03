using ActiveControlApi.DTO.Ativo;
using ActiveControlApi.Models;
using AtivoDTOModel = ActiveControlApi.DTO.Ativo.AtivoDTO;
using ModelAtivo = ActiveControlApi.Models.Ativo;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class AtivoDtoMapping
    {
        public static AtivoDTOModel? ParaAtivoDto(this ModelAtivo entity)
        {
            if (entity == null)
                return null;

            return new AtivoDTOModel
            {
                Id = entity.Id,
                AtivoNome = entity.AtivoNome,
                NumPatrimonio = entity.NumPatrimonio,
                NumSerie = entity.NumSerie,
                ValorAquisicao = entity.ValorAquisicao,
                DataAquisicao = entity.DataAquisicao,
                StatusAtivo = entity.StatusAtivo.HasValue ? (int)entity.StatusAtivo.Value : (int?)null,
                ModeloAtivoId = entity.ModeloAtivoId,
                CategoriaAtivoId = entity.CategoriaAtivoId,
                VidaUtilEstimadaAnos = entity.VidaUtilEstimadaAnos,
                TaxaDepreciacaoAnual = entity.TaxaDepreciacaoAnual
            };
        }

        public static ModelAtivo? ParaAtivo(this AtivoDTOModel dto)
        {
            if (dto == null)
                return null;

            return new ModelAtivo
            {
                Id = dto.Id,
                AtivoNome = dto.AtivoNome,
                NumPatrimonio = dto.NumPatrimonio,
                NumSerie = dto.NumSerie,
                ValorAquisicao = dto.ValorAquisicao,
                DataAquisicao = dto.DataAquisicao,
                StatusAtivo = dto.StatusAtivo.HasValue ? (Models.Enums.statusAtivo)dto.StatusAtivo.Value : null,
                ModeloAtivoId = dto.ModeloAtivoId,
                CategoriaAtivoId = dto.CategoriaAtivoId,
                VidaUtilEstimadaAnos = dto.VidaUtilEstimadaAnos,
                TaxaDepreciacaoAnual = dto.TaxaDepreciacaoAnual
            };
        }

        public static IEnumerable<AtivoDTOModel> ParaListaAtivoDto(this IEnumerable<ModelAtivo> entities)
        {
            if (entities == null || !entities.Any())
                return new List<AtivoDTOModel>();

            return entities.Select(e => e.ParaAtivoDto()!)
                           .ToList();
        }
    }
}


