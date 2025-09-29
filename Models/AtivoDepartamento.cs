using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    public class AtivoDepartamento
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Ativo")]
        public int AtivoId { get; set; }
        [JsonIgnore]
        public Ativo Ativo { get; set; }

        [Required]
        [ForeignKey("Departamento")]
        public int DepartamentoId { get; set; }
        [JsonIgnore]
        public Departamento Departamento { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }



    }
}
