using ActiveControlApi.DTO.Ativo;
using ActiveControlApi.Models;
using ModelAtivo = ActiveControlApi.Models.Ativo;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class AtivoDtoMapping
    {
        public static AtivoDTO? ParaAtivoDto(this ModelAtivo ativoRegistro)
        {
            if (ativoRegistro == null)
                return null;

            return new AtivoDTO
            {
                Id = ativoRegistro.Id,
                AtivoNome = ativoRegistro.AtivoNome,
                NumPatrimonio = ativoRegistro.NumPatrimonio,
                NumSerie = ativoRegistro.NumSerie,
                ValorAquisicao = ativoRegistro.ValorAquisicao,
                DataAquisicao = ativoRegistro.DataAquisicao,
                StatusAtivo = ativoRegistro.StatusAtivo,
                ModeloAtivoId = ativoRegistro.ModeloAtivoId,
                CategoriaAtivoId = ativoRegistro.CategoriaAtivoId,
                VidaUtilEstimadaAnos = ativoRegistro.VidaUtilEstimadaAnos,
                TaxaDepreciacaoAnual = ativoRegistro.TaxaDepreciacaoAnual
            };
        }

        public static ModelAtivo? ParaAtivo(this AtivoDTO ativoDtoRegistro)
        {
            if (ativoDtoRegistro == null)
                return null;

            return new ModelAtivo
            {
                Id = ativoDtoRegistro.Id ?? 0,
                AtivoNome = ativoDtoRegistro.AtivoNome,
                NumPatrimonio = ativoDtoRegistro.NumPatrimonio,
                NumSerie = ativoDtoRegistro.NumSerie,
                ValorAquisicao = ativoDtoRegistro.ValorAquisicao,
                DataAquisicao = ativoDtoRegistro.DataAquisicao,
                StatusAtivo = ativoDtoRegistro.StatusAtivo,
                ModeloAtivoId = ativoDtoRegistro.ModeloAtivoId,
                CategoriaAtivoId = ativoDtoRegistro.CategoriaAtivoId,
                VidaUtilEstimadaAnos = ativoDtoRegistro.VidaUtilEstimadaAnos,
                TaxaDepreciacaoAnual = ativoDtoRegistro.TaxaDepreciacaoAnual
            };
        }

        public static IEnumerable<AtivoDTO> ParaListaAtivoDto(this IEnumerable<ModelAtivo> ativoListRegistro)
        {
            if (ativoListRegistro == null || !ativoListRegistro.Any())
            {
                return new List<AtivoDTO>();
            }

            return ativoListRegistro.Select(a => new AtivoDTO
            {
                Id = a.Id,
                AtivoNome = a.AtivoNome,
                NumPatrimonio = a.NumPatrimonio,
                NumSerie = a.NumSerie,
                ValorAquisicao = a.ValorAquisicao,
                DataAquisicao = a.DataAquisicao,
                StatusAtivo = a.StatusAtivo,
                ModeloAtivoId = a.ModeloAtivoId,
                CategoriaAtivoId = a.CategoriaAtivoId,
                VidaUtilEstimadaAnos = a.VidaUtilEstimadaAnos,
                TaxaDepreciacaoAnual = a.TaxaDepreciacaoAnual
            }).ToList();
        }
    }
}
