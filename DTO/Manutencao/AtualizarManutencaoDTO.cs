using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.Manutencao
{
    public class AtualizarManutencaoDTO
    {
        public int? UsuarioResponsavelId { get; set; }

        public PrioridadeSolicitacao? Prioridade { get; set; }

        public DateTime? DataAgendada { get; set; }

        public DateTime? DataPrazo { get; set; }

        public decimal? CustoEstimado { get; set; }

        public decimal? CustoReal { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }

        [StringLength(500)]
        public string? SolucaoAplicada { get; set; }
    }
}







