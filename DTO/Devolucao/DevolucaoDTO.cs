using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.Devolucao
{
    public class DevolucaoDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Campo SolicitacaoId Obrigatório")]
        public int SolicitacaoId { get; set; }

        [StringLength(100)]
        public string? MotivoDevolucao { get; set; }
    }
}



