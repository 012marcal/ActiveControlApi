using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.Ativo
{
    public class AtivoDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Campo AtivoNome Obrigatório")]
        [StringLength(50)]
        public string AtivoNome { get; set; }

        [Required(ErrorMessage = "Campo NumPatrimonio Obrigatório")]
        [StringLength(50)]
        public string NumPatrimonio { get; set; }

        [Required(ErrorMessage = "Campo NumSerie Obrigatório")]
        [StringLength(45)]
        public string NumSerie { get; set; }

        [Required(ErrorMessage = "Campo ValorAquisicao Obrigatório")]
        public decimal ValorAquisicao { get; set; }

        [Required(ErrorMessage = "Campo DataAquisicao Obrigatório")]
        public DateTime DataAquisicao { get; set; }

        public statusAtivo? StatusAtivo { get; set; }

        [Required(ErrorMessage = "Campo ModeloAtivoId Obrigatório")]
        public int ModeloAtivoId { get; set; }

        [Required(ErrorMessage = "Campo CategoriaAtivoId Obrigatório")]
        public int CategoriaAtivoId { get; set; }

        [Required(ErrorMessage = "Campo VidaUtilEstimadaAnos Obrigatório")]
        [Range(1, 99, ErrorMessage = "A vida útil deve ser entre 1 e 99 anos.")]
        public int VidaUtilEstimadaAnos { get; set; }

        [Required(ErrorMessage = "Campo TaxaDepreciacaoAnual Obrigatório")]
        public decimal TaxaDepreciacaoAnual { get; set; }
    }
}
