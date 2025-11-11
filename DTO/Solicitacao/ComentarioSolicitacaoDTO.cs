namespace ActiveControlApi.DTO.Solicitacao
{
    public class ComentarioSolicitacaoDTO
    {
        public int Id { get; set; }
        public int SolicitacaoId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public string Comentario { get; set; }
        public DateTime DataComentario { get; set; }
        public bool Interno { get; set; }
    }
}

