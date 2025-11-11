using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.DTO.Solicitacao
{
    public class SolicitacaoDTO
    {
        public int? Id { get; set; }
        public string Titulo { get; set; }
        public int UsuarioSolicitanteId { get; set; }
        public string? UsuarioSolicitanteNome { get; set; }
        public int? UsuarioResponsavelId { get; set; }
        public string? UsuarioResponsavelNome { get; set; }
        public int AtivoId { get; set; }
        public string? AtivoNome { get; set; }
        public string? NumPatrimonio { get; set; }
        public TipoSolicitacao TipoSolicitacao { get; set; }
        public string Descricao { get; set; }
        public StatusSolicitacao? StatusSolicitacao { get; set; }
        public PrioridadeSolicitacao Prioridade { get; set; }
        public DateTime? DataAbertura { get; set; }
        public DateTime? DataPrazo { get; set; }
        public DateTime? DataFechamento { get; set; }
        public int? DiasAtraso { get; set; }
        public bool EstaAtrasada { get; set; }
    }
}


