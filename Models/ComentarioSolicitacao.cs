using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("ComentarioSolicitacao")]
    public class ComentarioSolicitacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Solicitacao")]
        public int SolicitacaoId { get; set; }
        [JsonIgnore]
        public Solicitacao Solicitacao { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comentario { get; set; }

        [Required]
        public DateTime DataComentario { get; set; } = DateTime.UtcNow;

        public bool Interno { get; set; } = false; // Se true, só Admin/SuperUser vê
    }
}

