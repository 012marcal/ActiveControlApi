using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.DTO.Historico
{
    public class HistoricoMovimentacaoDTO
    {
        public int Id { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
        public string TipoMovimentacaoDescricao { get; set; }
        public int AtivoId { get; set; }
        public string AtivoNome { get; set; }
        public string NumPatrimonio { get; set; }
        public int? UsuarioId { get; set; }
        public string? UsuarioNome { get; set; }
        public int? DepartamentoId { get; set; }
        public string? DepartamentoNome { get; set; }
        public int? SolicitacaoId { get; set; }
        public string Descricao { get; set; }
        public string? DadosAnteriores { get; set; }
        public string? DadosNovos { get; set; }
        public int UsuarioResponsavelId { get; set; }
        public string UsuarioResponsavelNome { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public string? IpAddress { get; set; }
    }
}


