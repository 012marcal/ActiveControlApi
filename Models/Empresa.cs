using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Globalization;

namespace ActiveControlApi.Models
{
    [Table("Empresas")]
    public class Empresa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RazaoSocial { get; set; }
        [Required]
        [StringLength(50)]
        public string NomeFantasia { get; set; }

        [Required]
        [StringLength(20)]
        public string Cnpj { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email Incorreto")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(40)]
        public string TelContato { get; set; }

        [StringLength(50)]
        public string? EnderecoEmpresa { get; set; }

        [Required]
        [StringLength(50)]
        public string CidadeEmpresa { get; set; }

        [Column(TypeName = "char(2)")]
        public string UfEmpresa { get; set; }
    }
}
