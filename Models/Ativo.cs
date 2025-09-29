using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("Ativos")]
    public class Ativo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string AtivoNome { get; set; }
        [Required]
        [StringLength(50)]
        public string NumPatrimonio { get; set; }
        [Required]
        [StringLength(45)]
        public string NumSerie { get; set; }
    
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorAquisicao { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime DataAquisicao { get; set; }
        public statusAtivo? statusAtivo { get; set; }
        [Required]
        [StringLength(50)]
        public int ModeloAtivoId { get; set; }
        [JsonIgnore]
        public ModeloAtivo  ModeloAtivo{ get; set; }


    }
}
