using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
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

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataFechamento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CustoEstimado { get; set; }

    }
}
