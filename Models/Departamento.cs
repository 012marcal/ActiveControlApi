using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("Departamentos")]
    public class Departamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Nome { get; set; }
        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        [JsonIgnore]
        public Empresa Empresa { get; set; }

        [StringLength(100)]
        public string EnderecoSetor { get; set; }

        [StringLength(100)]
        public string CidadeSetor { get; set; }

        [Column(TypeName = "char(2)")]
        public string UfSetor { get; set; }

        [StringLength(10)]
        public string Cep { get; set; }

        [StringLength(45)]
        public string LocalizacaoInterna { get; set; }
    }
}
