using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("Incidente")]
    public class Incidente
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Solicitacao")]
        [Required]
        public int SolicitacaoId { get; set; }
        [JsonIgnore]
        public Solicitacao Solicitacao { get; set; }
        [StringLength(100)]
        public string Severidade    { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }


        
        
    }
}
