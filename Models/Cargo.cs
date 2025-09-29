using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.Models
{
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
