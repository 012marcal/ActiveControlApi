using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    public class Manutencao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SolicitacaoId { get; set; }
        [JsonIgnore]
        public Solicitacao Solicitacao { get; set; }[
    }
}
