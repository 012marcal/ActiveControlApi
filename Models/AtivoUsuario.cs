using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    public class AtivoUsuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Ativo")]
        public int AtivoId { get; set; }
        [JsonIgnore]
        public Ativo Ativo { get; set; }
        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }
        [Required]
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

    }
}
