using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("Incidente")]
    public class Incidente
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Solicitacao")]
        [Required]
        public int SolicitacaoId { get; set; }
        [JsonIgnore]
        public Solicitacao Solicitacao { get; set; }

        public SeveridadeIncidente Severidade { get; set; } = SeveridadeIncidente.Media;

        public StatusIncidente StatusIncidente { get; set; } = StatusIncidente.Aberto;

        [ForeignKey("UsuarioResponsavel")]
        public int? UsuarioResponsavelId { get; set; }
        [JsonIgnore]
        public Usuario? UsuarioResponsavel { get; set; }

        public PrioridadeSolicitacao Prioridade { get; set; } = PrioridadeSolicitacao.Media;

        [Required]
        [StringLength(1000)]
        public string Descricao { get; set; }

        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
        public DateTime? DataInicioResolucao { get; set; }
        public DateTime? DataResolucao { get; set; }
        public DateTime? DataPrazo { get; set; }

        [StringLength(1000)]
        public string? SolucaoAplicada { get; set; }

        [StringLength(500)]
        public string? CausaRaiz { get; set; }
    }
}
