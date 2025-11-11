using ActiveControlApi.DTO.Solicitacao;
using ActiveControlApi.Models;
using ActiveControlApi.Models.Enums;
using ModelSolicitacao = ActiveControlApi.Models.Solicitacao;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class SolicitacaoDtoMapping
    {
        public static SolicitacaoDTO? ParaSolicitacaoDto(this ModelSolicitacao solicitacaoRegistro)
        {
            if (solicitacaoRegistro == null)
                return null;

            var diasAtraso = 0;
            var estaAtrasada = false;
            if (solicitacaoRegistro.DataPrazo.HasValue && 
                solicitacaoRegistro.StatusSolicitacao != StatusSolicitacao.FINALIZADA &&
                solicitacaoRegistro.StatusSolicitacao != StatusSolicitacao.CANCELADA)
            {
                var dias = (DateTime.UtcNow - solicitacaoRegistro.DataPrazo.Value).Days;
                if (dias > 0)
                {
                    diasAtraso = dias;
                    estaAtrasada = true;
                }
            }

            return new SolicitacaoDTO
            {
                Id = solicitacaoRegistro.Id,
                Titulo = solicitacaoRegistro.Titulo,
                UsuarioSolicitanteId = solicitacaoRegistro.UsuarioSolicitanteId,
                UsuarioSolicitanteNome = solicitacaoRegistro.Usuario?.NomeCompleto,
                UsuarioResponsavelId = solicitacaoRegistro.UsuarioResponsavelId,
                UsuarioResponsavelNome = solicitacaoRegistro.UsuarioResponsavel?.NomeCompleto,
                AtivoId = solicitacaoRegistro.AtivoId,
                AtivoNome = solicitacaoRegistro.Ativo?.AtivoNome,
                NumPatrimonio = solicitacaoRegistro.Ativo?.NumPatrimonio,
                TipoSolicitacao = solicitacaoRegistro.TipoSolicitacao,
                Descricao = solicitacaoRegistro.Descricao,
                StatusSolicitacao = solicitacaoRegistro.StatusSolicitacao,
                Prioridade = solicitacaoRegistro.Prioridade,
                DataAbertura = solicitacaoRegistro.DataAbertura,
                DataPrazo = solicitacaoRegistro.DataPrazo,
                DataFechamento = solicitacaoRegistro.DataFechamento,
                DiasAtraso = diasAtraso > 0 ? diasAtraso : null,
                EstaAtrasada = estaAtrasada
            };
        }

        public static ModelSolicitacao? ParaSolicitacao(this SolicitacaoDTO solicitacaoDtoRegistro)
        {
            if (solicitacaoDtoRegistro == null)
                return null;

            return new ModelSolicitacao
            {
                Id = solicitacaoDtoRegistro.Id ?? 0,
                Titulo = solicitacaoDtoRegistro.Titulo ?? string.Empty,
                UsuarioSolicitanteId = solicitacaoDtoRegistro.UsuarioSolicitanteId,
                UsuarioResponsavelId = solicitacaoDtoRegistro.UsuarioResponsavelId,
                AtivoId = solicitacaoDtoRegistro.AtivoId,
                TipoSolicitacao = solicitacaoDtoRegistro.TipoSolicitacao,
                Descricao = solicitacaoDtoRegistro.Descricao,
                StatusSolicitacao = solicitacaoDtoRegistro.StatusSolicitacao,
                Prioridade = solicitacaoDtoRegistro.Prioridade,
                DataAbertura = solicitacaoDtoRegistro.DataAbertura.HasValue ? solicitacaoDtoRegistro.DataAbertura.Value : DateTime.UtcNow,
                DataPrazo = solicitacaoDtoRegistro.DataPrazo,
                DataFechamento = solicitacaoDtoRegistro.DataFechamento
            };
        }

        public static IEnumerable<SolicitacaoDTO> ParaListaSolicitacaoDto(this IEnumerable<ModelSolicitacao> solicitacaoListRegistro)
        {
            if (solicitacaoListRegistro == null || !solicitacaoListRegistro.Any())
            {
                return new List<SolicitacaoDTO>();
            }

            return solicitacaoListRegistro.Select(s => s.ParaSolicitacaoDto()!).Where(d => d != null).ToList();
        }
    }
}

