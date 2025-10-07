using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActiveControlApi.Models
{
    [Table("Empresa")]
    public class Empresa
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Campo Razao Social Obrigatorio")]
        public string RazaoSocial { get; set; }

        [StringLength(50)]
        public string? NomeFantasia { get; set; }

        [Required(ErrorMessage = "Campo Email Obrigatorio")]
        [StringLength(14)]
        public string Cnpj { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email Incorreto")]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(40)]
        public string? TelContato { get; set; }

        [StringLength(50)]
        public string? EnderecoEmpresa { get; set; }

        [Required(ErrorMessage = "Defina a Cidade da Empresa")]
        [StringLength(50)]
        public string CidadeEmpresa { get; set; }


        [Required(ErrorMessage = "Campo UF empresa Obrigatório")]
        [Column(TypeName = "char(2)")]
        public string UfEmpresa { get; set; }
        [JsonIgnore]
        public ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();
    }
}
