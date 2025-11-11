using ActiveControlApi.DTO.Incidente;
using ActiveControlApi.Models;
using ActiveControlApi.Models.Enums;
using ModelIncidente = ActiveControlApi.Models.Incidente;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class IncidenteDtoMapping
    {
        public static IncidenteDTO? ParaIncidenteDto(this ModelIncidente incidenteRegistro)
        {
            if (incidenteRegistro == null)
                return null;

            var diasAtraso = 0;
            var estaAtrasada = false;
            if (incidenteRegistro.DataPrazo.HasValue &&
                incidenteRegistro.StatusIncidente != StatusIncidente.Resolvido &&
                incidenteRegistro.StatusIncidente != StatusIncidente.Cancelado)
            {
                var dias = (DateTime.UtcNow - incidenteRegistro.DataPrazo.Value).Days;
                if (dias > 0)
                {
                    diasAtraso = dias;
                    estaAtrasada = true;
                }
            }

            return new IncidenteDTO
            {
                Id = incidenteRegistro.Id,
                SolicitacaoId = incidenteRegistro.SolicitacaoId,
                SolicitacaoTitulo = incidenteRegistro.Solicitacao?.Titulo,
                AtivoId = incidenteRegistro.Solicitacao?.AtivoId,
                AtivoNome = incidenteRegistro.Solicitacao?.Ativo?.AtivoNome,
                NumPatrimonio = incidenteRegistro.Solicitacao?.Ativo?.NumPatrimonio,
                Severidade = incidenteRegistro.Severidade,
                StatusIncidente = incidenteRegistro.StatusIncidente,
                UsuarioResponsavelId = incidenteRegistro.UsuarioResponsavelId,
                UsuarioResponsavelNome = incidenteRegistro.UsuarioResponsavel?.NomeCompleto,
                Prioridade = incidenteRegistro.Prioridade,
                Descricao = incidenteRegistro.Descricao,
                DataAbertura = incidenteRegistro.DataAbertura,
                DataInicioResolucao = incidenteRegistro.DataInicioResolucao,
                DataResolucao = incidenteRegistro.DataResolucao,
                DataPrazo = incidenteRegistro.DataPrazo,
                SolucaoAplicada = incidenteRegistro.SolucaoAplicada,
                CausaRaiz = incidenteRegistro.CausaRaiz,
                DiasAtraso = diasAtraso > 0 ? diasAtraso : null,
                EstaAtrasada = estaAtrasada
            };
        }

        public static ModelIncidente? ParaIncidente(this IncidenteDTO incidenteDtoRegistro)
        {
            if (incidenteDtoRegistro == null)
                return null;

            return new ModelIncidente
            {
                Id = incidenteDtoRegistro.Id ?? 0,
                SolicitacaoId = incidenteDtoRegistro.SolicitacaoId,
                Severidade = incidenteDtoRegistro.Severidade,
                StatusIncidente = incidenteDtoRegistro.StatusIncidente,
                UsuarioResponsavelId = incidenteDtoRegistro.UsuarioResponsavelId,
                Prioridade = incidenteDtoRegistro.Prioridade,
                Descricao = incidenteDtoRegistro.Descricao ?? string.Empty,
                DataAbertura = incidenteDtoRegistro.DataAbertura ?? DateTime.UtcNow,
                DataInicioResolucao = incidenteDtoRegistro.DataInicioResolucao,
                DataResolucao = incidenteDtoRegistro.DataResolucao,
                DataPrazo = incidenteDtoRegistro.DataPrazo,
                SolucaoAplicada = incidenteDtoRegistro.SolucaoAplicada,
                CausaRaiz = incidenteDtoRegistro.CausaRaiz
            };
        }

        public static IEnumerable<IncidenteDTO> ParaListaIncidenteDto(this IEnumerable<ModelIncidente> incidenteListRegistro)
        {
            if (incidenteListRegistro == null || !incidenteListRegistro.Any())
            {
                return new List<IncidenteDTO>();
            }

            return incidenteListRegistro.Select(i => i.ParaIncidenteDto()!).Where(d => d != null).ToList();
        }
    }
}

