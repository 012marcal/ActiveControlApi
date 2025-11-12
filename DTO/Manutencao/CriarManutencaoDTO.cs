using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.Manutencao
{
    public class CriarManutencaoDTO
    {
        [Required(ErrorMessage = "Solicitação é obrigatória")]
        public int SolicitacaoId { get; set; }

        [Required(ErrorMessage = "Tipo de manutenção é obrigatório")]
        public TipoManutencao TipoManutencao { get; set; }

        public int? UsuarioResponsavelId { get; set; }

        public PrioridadeSolicitacao Prioridade { get; set; } = PrioridadeSolicitacao.Media;

        public DateTime? DataAgendada { get; set; }

        public DateTime? DataPrazo { get; set; }

        public decimal? CustoEstimado { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}

