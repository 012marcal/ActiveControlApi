using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.Cargo
{
    public class CargoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Descrição do cargo é obrigatória")]
        [StringLength(100)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "CBO é obrigatório")]
        [StringLength(8)]
        public string CBO { get; set; }
    }

    public class CriarCargoDTO
    {
        [Required(ErrorMessage = "Descrição do cargo é obrigatória")]
        [StringLength(100)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "CBO é obrigatório")]
        [StringLength(8)]
        public string CBO { get; set; }
    }

    public class AtualizarCargoDTO
    {
        [Required(ErrorMessage = "Descrição do cargo é obrigatória")]
        [StringLength(100)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "CBO é obrigatório")]
        [StringLength(8)]
        public string CBO { get; set; }
    }
}





