using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.ModeloAtivo
{
    public class ModeloAtivoDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Fabricante { get; set; }

        [Required]
        [StringLength(200)]
        public string Especificacoes { get; set; }
    }
}





