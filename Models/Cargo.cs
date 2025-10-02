using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActiveControlApi.Models
{
    [Table("Cargo")]
    public class Cargo 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        [StringLength(8)]
        public string CBO { get; set; }
    }
}
