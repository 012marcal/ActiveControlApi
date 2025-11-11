using ActiveControlApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ActiveControlApi.DTO.Manutencao
{
    public class ManutencaoDTO
    {
        public int? Id { get; set; }
        public int SolicitacaoId { get; set; }
        public string? SolicitacaoTitulo { get; set; }
        public int? AtivoId { get; set; }
        public string? AtivoNome { get; set; }
        public string? NumPatrimonio { get; set; }
        public TipoManutencao TipoManutencao { get; set; }
        public StatusManutencao StatusManutencao { get; set; }
        public int? UsuarioResponsavelId { get; set; }
        public string? UsuarioResponsavelNome { get; set; }
        public PrioridadeSolicitacao Prioridade { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAgendada { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFechamento { get; set; }
        public DateTime? DataPrazo { get; set; }
        public decimal? CustoEstimado { get; set; }
        public decimal? CustoReal { get; set; }
        public string? Observacoes { get; set; }
        public string? SolucaoAplicada { get; set; }
        public int? DiasAtraso { get; set; }
        public bool EstaAtrasada { get; set; }
    }
}



