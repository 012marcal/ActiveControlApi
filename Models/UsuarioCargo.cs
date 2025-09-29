using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    public class UsuarioCargo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }

        [Required]
        [ForeignKey("Cargo")]
        public int CargoId { get; set; }
        [JsonIgnore]
        public Cargo Cargo { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
