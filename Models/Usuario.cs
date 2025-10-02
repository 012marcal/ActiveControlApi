using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("Usuario")]
    public class Usuario 
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NomeCompleto { get; set; }
        [Required]
        [StringLength(12)]
        public string Cpf { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email Invalido")]
        public string Email { get; set; }
        [JsonIgnore]
        public byte[] SenhaHash { get; set; }
        [JsonIgnore]
        public byte[] SenhaSalt { get; set; }
        public DateTime TokenDataCriacao { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<UsuarioCargo> CargosHistoricos { get; set; } = new List<UsuarioCargo>();


    }
}
