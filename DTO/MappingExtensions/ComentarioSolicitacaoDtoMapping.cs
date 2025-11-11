using ActiveControlApi.DTO.Solicitacao;
using ActiveControlApi.Models;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class ComentarioSolicitacaoDtoMapping
    {
        public static ComentarioSolicitacaoDTO ParaDto(this ComentarioSolicitacao comentario)
        {
            return new ComentarioSolicitacaoDTO
            {
                Id = comentario.Id,
                SolicitacaoId = comentario.SolicitacaoId,
                UsuarioId = comentario.UsuarioId,
                UsuarioNome = comentario.Usuario?.NomeCompleto ?? "N/A",
                Comentario = comentario.Comentario,
                DataComentario = comentario.DataComentario,
                Interno = comentario.Interno
            };
        }

        public static IEnumerable<ComentarioSolicitacaoDTO> ParaListaDto(this IEnumerable<ComentarioSolicitacao> comentarios)
        {
            return comentarios.Select(c => c.ParaDto());
        }

        public static ComentarioSolicitacao ParaEntity(this ComentarioSolicitacaoDTO dto)
        {
            return new ComentarioSolicitacao
            {
                Id = dto.Id,
                SolicitacaoId = dto.SolicitacaoId,
                UsuarioId = dto.UsuarioId,
                Comentario = dto.Comentario,
                DataComentario = dto.DataComentario,
                Interno = dto.Interno
            };
        }
    }
}

