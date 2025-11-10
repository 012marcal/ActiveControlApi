using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.AtivoDepartamento
{
    public class AtivoDepartamentoDTO
    {
        public int Id { get; set; }

        [Required]
        public int AtivoId { get; set; }

        [Required]
        public int DepartamentoId { get; set; }

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}


