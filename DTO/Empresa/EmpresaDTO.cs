using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActiveControlApi.DTO.Empresas
{
    public class EmpresaDTO
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Campo Razao Social Obrigatorio")]
        public string RazaoSocial { get; set; }

        [StringLength(50)]
        public string? NomeFantasia { get; set; }

        [Required (ErrorMessage = "Campo Cnpj Obrigatorio")]
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

        [Required(ErrorMessage ="Defina a Cidade da Empresa")]
        [StringLength(50)]
        public string CidadeEmpresa { get; set; }


        [Required (ErrorMessage = "Campo UF empresa Obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "A UF deve ter exatamente 2 caracteres (sigla).")]
        public string UfEmpresa { get; set; }
    }
}
 