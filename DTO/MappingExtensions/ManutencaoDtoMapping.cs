using ActiveControlApi.DTO.Manutencao;
using ActiveControlApi.Models;
using ActiveControlApi.Models.Enums;
using ModelManutencao = ActiveControlApi.Models.Manutencao;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class ManutencaoDtoMapping
    {
        public static ManutencaoDTO? ParaManutencaoDto(this ModelManutencao manutencaoRegistro)
        {
            if (manutencaoRegistro == null)
                return null;

            var diasAtraso = 0;
            var estaAtrasada = false;
            if (manutencaoRegistro.DataPrazo.HasValue &&
                manutencaoRegistro.StatusManutencao != StatusManutencao.Concluida &&
                manutencaoRegistro.StatusManutencao != StatusManutencao.Cancelada)
            {
                var dias = (DateTime.UtcNow - manutencaoRegistro.DataPrazo.Value).Days;
                if (dias > 0)
                {
                    diasAtraso = dias;
                    estaAtrasada = true;
                }
            }

            return new ManutencaoDTO
            {
                Id = manutencaoRegistro.Id,
                SolicitacaoId = manutencaoRegistro.SolicitacaoId,
                SolicitacaoTitulo = manutencaoRegistro.Solicitacao?.Titulo,
                AtivoId = manutencaoRegistro.Solicitacao?.AtivoId,
                AtivoNome = manutencaoRegistro.Solicitacao?.Ativo?.AtivoNome,
                NumPatrimonio = manutencaoRegistro.Solicitacao?.Ativo?.NumPatrimonio,
                TipoManutencao = manutencaoRegistro.TipoManutencao,
                StatusManutencao = manutencaoRegistro.StatusManutencao,
                UsuarioResponsavelId = manutencaoRegistro.UsuarioResponsavelId,
                UsuarioResponsavelNome = manutencaoRegistro.UsuarioResponsavel?.NomeCompleto,
                Prioridade = manutencaoRegistro.Prioridade,
                DataCriacao = manutencaoRegistro.DataCriacao,
                DataAgendada = manutencaoRegistro.DataAgendada,
                DataInicio = manutencaoRegistro.DataInicio,
                DataFechamento = manutencaoRegistro.DataFechamento,
                DataPrazo = manutencaoRegistro.DataPrazo,
                CustoEstimado = manutencaoRegistro.CustoEstimado,
                CustoReal = manutencaoRegistro.CustoReal,
                Observacoes = manutencaoRegistro.Observacoes,
                SolucaoAplicada = manutencaoRegistro.SolucaoAplicada,
                DiasAtraso = diasAtraso > 0 ? diasAtraso : null,
                EstaAtrasada = estaAtrasada
            };
        }

        public static ModelManutencao? ParaManutencao(this ManutencaoDTO manutencaoDtoRegistro)
        {
            if (manutencaoDtoRegistro == null)
                return null;

            return new ModelManutencao
            {
                Id = manutencaoDtoRegistro.Id ?? 0,
                SolicitacaoId = manutencaoDtoRegistro.SolicitacaoId,
                TipoManutencao = manutencaoDtoRegistro.TipoManutencao,
                StatusManutencao = manutencaoDtoRegistro.StatusManutencao,
                UsuarioResponsavelId = manutencaoDtoRegistro.UsuarioResponsavelId,
                Prioridade = manutencaoDtoRegistro.Prioridade,
                DataCriacao = manutencaoDtoRegistro.DataCriacao ?? DateTime.UtcNow,
                DataAgendada = manutencaoDtoRegistro.DataAgendada,
                DataInicio = manutencaoDtoRegistro.DataInicio,
                DataFechamento = manutencaoDtoRegistro.DataFechamento,
                DataPrazo = manutencaoDtoRegistro.DataPrazo,
                CustoEstimado = manutencaoDtoRegistro.CustoEstimado,
                CustoReal = manutencaoDtoRegistro.CustoReal,
                Observacoes = manutencaoDtoRegistro.Observacoes,
                SolucaoAplicada = manutencaoDtoRegistro.SolucaoAplicada
            };
        }

        public static IEnumerable<ManutencaoDTO> ParaListaManutencaoDto(this IEnumerable<ModelManutencao> manutencaoListRegistro)
        {
            if (manutencaoListRegistro == null || !manutencaoListRegistro.Any())
            {
                return new List<ManutencaoDTO>();
            }

            return manutencaoListRegistro.Select(m => m.ParaManutencaoDto()!).Where(d => d != null).ToList();
        }
    }
}

