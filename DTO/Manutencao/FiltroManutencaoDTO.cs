using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.DTO.Manutencao
{
    public class FiltroManutencaoDTO
    {
        public StatusManutencao? Status { get; set; }
        public TipoManutencao? Tipo { get; set; }
        public PrioridadeSolicitacao? Prioridade { get; set; }
        public int? AtivoId { get; set; }
        public int? UsuarioResponsavelId { get; set; }
        public int? SolicitacaoId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool? Atrasadas { get; set; }
    }
}







