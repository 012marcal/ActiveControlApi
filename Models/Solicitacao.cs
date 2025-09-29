using ActiveControlApi.Models.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public TipoSolicitacao TipoSolicitacao { get; set; }

        [Required]
        [StringLength(300)]
        public string Descricao { get; set; }   

        public StatusSolicitacao? StatusSolicitacao { get; set; }

        public DateTime DataAbertura { get; set; }
        public DateTime DataFechamento { get; set; }

    }
}
