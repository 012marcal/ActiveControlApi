using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.DTO.Incidente
{
    public class IncidenteDTO
    {
        public int? Id { get; set; }
        public int SolicitacaoId { get; set; }
        public string? SolicitacaoTitulo { get; set; }
        public int? AtivoId { get; set; }
        public string? AtivoNome { get; set; }
        public string? NumPatrimonio { get; set; }
        public SeveridadeIncidente Severidade { get; set; }
        public StatusIncidente StatusIncidente { get; set; }
        public int? UsuarioResponsavelId { get; set; }
        public string? UsuarioResponsavelNome { get; set; }
        public PrioridadeSolicitacao Prioridade { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataAbertura { get; set; }
        public DateTime? DataInicioResolucao { get; set; }
        public DateTime? DataResolucao { get; set; }
        public DateTime? DataPrazo { get; set; }
        public string? SolucaoAplicada { get; set; }
        public string? CausaRaiz { get; set; }
        public int? DiasAtraso { get; set; }
        public bool EstaAtrasada { get; set; }
    }
}



