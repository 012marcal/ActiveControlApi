using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Observacoes { get; set; }
    }
}
