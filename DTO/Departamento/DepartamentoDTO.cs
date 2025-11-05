using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.Departamento
{
    public class DepartamentoDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Nome { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [StringLength(100)]
        public string? EnderecoSetor { get; set; }

        [StringLength(100)]
        public string? CidadeSetor { get; set; }

        [StringLength(2)]
        public string? UfSetor { get; set; }

        [StringLength(10)]
        public string? Cep { get; set; }

        [StringLength(45)]
        public string? LocalizacaoInterna { get; set; }
    }
}





