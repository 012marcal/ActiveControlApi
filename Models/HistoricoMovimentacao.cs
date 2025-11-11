using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("HistoricoMovimentacao")]
    public class HistoricoMovimentacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TipoMovimentacao TipoMovimentacao { get; set; }

        [Required]
        [ForeignKey("Ativo")]
        public int AtivoId { get; set; }
        [JsonIgnore]
        public Ativo Ativo { get; set; }

        [ForeignKey("Usuario")]
        public int? UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [ForeignKey("Departamento")]
        public int? DepartamentoId { get; set; }
        [JsonIgnore]
        public Departamento? Departamento { get; set; }

        [ForeignKey("Solicitacao")]
        public int? SolicitacaoId { get; set; }
        [JsonIgnore]
        public Solicitacao? Solicitacao { get; set; }

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; }

        [StringLength(1000)]
        public string? DadosAnteriores { get; set; }

        [StringLength(1000)]
        public string? DadosNovos { get; set; }

        [Required]
        [ForeignKey("UsuarioResponsavel")]
        public int UsuarioResponsavelId { get; set; }
        [JsonIgnore]
        public Usuario UsuarioResponsavel { get; set; }

        [Required]
        public DateTime DataMovimentacao { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string? IpAddress { get; set; }
    }
}


