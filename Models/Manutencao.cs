using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("Manutencao")]
    public class Manutencao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SolicitacaoId { get; set; }

        [JsonIgnore]
        public Solicitacao Solicitacao { get; set; }

        [Required]
        public TipoManutencao TipoManutencao { get; set; }

        public StatusManutencao StatusManutencao { get; set; } = StatusManutencao.Agendada;

        [ForeignKey("UsuarioResponsavel")]
        public int? UsuarioResponsavelId { get; set; }
        [JsonIgnore]
        public Usuario? UsuarioResponsavel { get; set; }

        public PrioridadeSolicitacao Prioridade { get; set; } = PrioridadeSolicitacao.Media;

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAgendada { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFechamento { get; set; }
        public DateTime? DataPrazo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CustoEstimado { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CustoReal { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }

        [StringLength(500)]
        public string? SolucaoAplicada { get; set; }
    }
}
