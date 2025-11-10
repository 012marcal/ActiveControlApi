using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.CategoriaAtivo
{
    public class CategoriaAtivoDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeCategoria { get; set; }
    }
}


