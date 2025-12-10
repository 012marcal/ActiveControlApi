using ActiveControlApi.DTO.CategoriaAtivo;
using ActiveControlApi.DTO.ModeloAtivo;
using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.DTO.Ativo
{


        public class AtivoCompletoDTO
        {
            public int Id { get; set; }
            public string AtivoNome { get; set; }
            public string NumPatrimonio { get; set; }
            public string NumSerie { get; set; }
            public decimal ValorAquisicao { get; set; }
            public DateTime DataAquisicao { get; set; }
            public statusAtivo? StatusAtivo { get; set; }
            public ModeloAtivoDTO Modelo { get; set; }
            public CategoriaAtivoDTO Categoria { get; set; }
            public int VidaUtilEstimadaAnos { get; set; }
            public decimal TaxaDepreciacaoAnual { get; set; }
            public ResponsavelAtivoDTO? Responsavel { get; set; }



    }



}
