using ActiveControlApi.DTO.Historico;
using ActiveControlApi.Models;
using ActiveControlApi.Models.Enums;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class HistoricoMovimentacaoDtoMapping
    {
        public static HistoricoMovimentacaoDTO ParaDto(this HistoricoMovimentacao historico)
        {
            return new HistoricoMovimentacaoDTO
            {
                Id = historico.Id,
                TipoMovimentacao = historico.TipoMovimentacao,
                TipoMovimentacaoDescricao = ObterDescricaoTipoMovimentacao(historico.TipoMovimentacao),
                AtivoId = historico.AtivoId,
                AtivoNome = historico.Ativo?.AtivoNome ?? "N/A",
                NumPatrimonio = historico.Ativo?.NumPatrimonio ?? "N/A",
                UsuarioId = historico.UsuarioId,
                UsuarioNome = historico.Usuario?.NomeCompleto,
                DepartamentoId = historico.DepartamentoId,
                DepartamentoNome = historico.Departamento?.Nome,
                SolicitacaoId = historico.SolicitacaoId,
                Descricao = historico.Descricao,
                DadosAnteriores = historico.DadosAnteriores,
                DadosNovos = historico.DadosNovos,
                UsuarioResponsavelId = historico.UsuarioResponsavelId,
                UsuarioResponsavelNome = historico.UsuarioResponsavel?.NomeCompleto ?? "N/A",
                DataMovimentacao = historico.DataMovimentacao,
                IpAddress = historico.IpAddress
            };
        }

        public static IEnumerable<HistoricoMovimentacaoDTO> ParaListaDto(this IEnumerable<HistoricoMovimentacao> historicos)
        {
            return historicos.Select(h => h.ParaDto());
        }

        private static string ObterDescricaoTipoMovimentacao(TipoMovimentacao tipo)
        {
            return tipo switch
            {
                TipoMovimentacao.Criacao => "Criação",
                TipoMovimentacao.Atualizacao => "Atualização",
                TipoMovimentacao.Exclusao => "Exclusão",
                TipoMovimentacao.AlocacaoUsuario => "Alocação para Usuário",
                TipoMovimentacao.DesalocacaoUsuario => "Desalocação de Usuário",
                TipoMovimentacao.AlocacaoDepartamento => "Alocação para Departamento",
                TipoMovimentacao.DesalocacaoDepartamento => "Desalocação de Departamento",
                TipoMovimentacao.AlteracaoStatus => "Alteração de Status",
                TipoMovimentacao.AlteracaoValor => "Alteração de Valor",
                TipoMovimentacao.CriacaoSolicitacao => "Criação de Solicitação",
                TipoMovimentacao.AtualizacaoSolicitacao => "Atualização de Solicitação",
                TipoMovimentacao.FinalizacaoSolicitacao => "Finalização de Solicitação",
                _ => "Desconhecido"
            };
        }
    }
}

