using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    public abstract class BaseEntity
    {
        public DateTime DataCriacao { get; set; }
        [ForeignKey("UsuarioCriacaoId")]
        public int UsuarioCriacaoId { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        [ForeignKey("UsuarioAlteracaoId")]
        public int? UsuarioAlteracaoId { get; set; }
        [JsonIgnore]
        public Usuario UsuarioAlteracao { get; set; }
    }
}
}
