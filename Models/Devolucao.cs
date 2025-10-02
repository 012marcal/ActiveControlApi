using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("Devolucao")]
    public class Devolucao
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Solicitacao")]
        public int SolicitacaoId { get; set; }
        
        [JsonIgnore]
        public Solicitacao Solicitacao { get; set; }

        [StringLength(100)]
        public string MotivoDevolucao { get; set; }


    }
}
