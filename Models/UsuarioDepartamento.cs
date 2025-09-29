using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    public class UsuarioDepartamento
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Departamento")]
        public int DepartamentoId { get; set; }

        [JsonIgnore]
        public Departamento Departamento { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario Usuario  { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }    

        public DateTime? DataFim {  get; set; }


    }
}
