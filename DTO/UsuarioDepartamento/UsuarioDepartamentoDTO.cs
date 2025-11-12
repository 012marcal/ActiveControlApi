using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.UsuarioDepartamento
{
    public class UsuarioDepartamentoDTO
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public string? UsuarioNome { get; set; }

        [Required]
        public int DepartamentoId { get; set; }
        public string? DepartamentoNome { get; set; }
        public int? EmpresaId { get; set; }
        public string? EmpresaNome { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }

    public class CriarUsuarioDepartamentoDTO
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Departamento é obrigatório")]
        public int DepartamentoId { get; set; }

        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }

    public class AtualizarUsuarioDepartamentoDTO
    {
        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}





