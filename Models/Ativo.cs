using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("Ativo")]
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
        public statusAtivo? StatusAtivo { get; set; }
        [Required]
        [ForeignKey("ModeloAtivo")]
        public int ModeloAtivoId { get; set; }
        [JsonIgnore]
        public ModeloAtivo ModeloAtivo { get; set; }

        [Required]
        [ForeignKey("CategoriaAtivo")]
        public int CategoriaAtivoId { get; set; }

        [JsonIgnore]
        public CategoriaAtivo CategoriaAtivo { get; set; }

        [Required]
        [Range(1, 99, ErrorMessage = "A vida útil deve ser entre 1 e 99 anos.")]
        public int VidaUtilEstimadaAnos { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxaDepreciacaoAnual {  get; set; }

        [JsonIgnore]
        public ICollection<AtivoUsuario> AtivoUsuario { get; set; } = new List<AtivoUsuario>();

        [JsonIgnore]
        public ICollection<AtivoDepartamento> AtivoDepartamento { get; set; } = new List<AtivoDepartamento>();


    }
}
