using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.Usuario
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeCompleto { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string Cpf { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(120)]
        public string Email { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 6)]
        public string Senha { get; set; }

        public Role Role { get; set; } = Role.User;
    }
}


