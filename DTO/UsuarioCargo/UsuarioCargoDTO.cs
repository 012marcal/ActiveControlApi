using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.UsuarioCargo
{
    public class UsuarioCargoDTO
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public string? UsuarioNome { get; set; }

        [Required]
        public int CargoId { get; set; }
        public string? CargoDescricao { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }

    public class CriarUsuarioCargoDTO
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Cargo é obrigatório")]
        public int CargoId { get; set; }

        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }

    public class AtualizarUsuarioCargoDTO
    {
        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}





