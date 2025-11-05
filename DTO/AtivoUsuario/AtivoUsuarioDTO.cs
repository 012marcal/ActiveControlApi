using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.AtivoUsuario
{
    public class AtivoUsuarioDTO
    {
        public int Id { get; set; }

        [Required]
        public int AtivoId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}





