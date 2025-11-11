using ActiveControlApi.DTO.Devolucao;
using ActiveControlApi.Models;
using ModelDevolucao = ActiveControlApi.Models.Devolucao;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class DevolucaoDtoMapping
    {
        public static DevolucaoDTO? ParaDevolucaoDto(this ModelDevolucao devolucaoRegistro)
        {
            if (devolucaoRegistro == null)
                return null;

            return new DevolucaoDTO
            {
                Id = devolucaoRegistro.Id,
                SolicitacaoId = devolucaoRegistro.SolicitacaoId,
                MotivoDevolucao = devolucaoRegistro.MotivoDevolucao
            };
        }

        public static ModelDevolucao? ParaDevolucao(this DevolucaoDTO devolucaoDtoRegistro)
        {
            if (devolucaoDtoRegistro == null)
                return null;

            return new ModelDevolucao
            {
                Id = devolucaoDtoRegistro.Id ?? 0,
                SolicitacaoId = devolucaoDtoRegistro.SolicitacaoId,
                MotivoDevolucao = devolucaoDtoRegistro.MotivoDevolucao
            };
        }

        public static IEnumerable<DevolucaoDTO> ParaListaDevolucaoDto(this IEnumerable<ModelDevolucao> devolucaoListRegistro)
        {
            if (devolucaoListRegistro == null || !devolucaoListRegistro.Any())
            {
                return new List<DevolucaoDTO>();
            }

            return devolucaoListRegistro.Select(d => new DevolucaoDTO
            {
                Id = d.Id,
                SolicitacaoId = d.SolicitacaoId,
                MotivoDevolucao = d.MotivoDevolucao
            }).ToList();
        }
    }
}

