using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActiveControlApi.Models
{
    [Table("CategoriaAtivo")]
    public class CategoriaAtivo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string NomeCategoria { get; set; }
    }
}
