using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("ModeloAtivo")]
    public class ModeloAtivo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nome { get; set; }
        [Required]
        [StringLength(50)]
        public string Fabricante { get; set; }
        [Required]
        [StringLength(50)]
        public string Especificacoes { get; set; }

        [JsonIgnore]
        public ICollection<Ativo> Ativos { get; set; } = new List<Ativo>();
    }
}
  