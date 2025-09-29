using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    public class Solicitacao
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("UsuarioSolicitante")]
        public int UsuarioSolicitanteId { get; set; }
        [JsonIgnore]
        public Usuario Usuario {  get; set; }
        [Required]
        [ForeignKey("Ativo")]
        public int AtivoId { get; set; }
        [JsonIgnore]
        public Ativo Ativo { get; set; }
        [Required]
    }
}
