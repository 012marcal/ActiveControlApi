using ActiveControlApi.DTO.Solicitacao;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Models;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;
using ModelSolicitacao = ActiveControlApi.Models.Solicitacao;
using ModelIncidente = ActiveControlApi.Models.Incidente;
using ModelManutencao = ActiveControlApi.Models.Manutencao;
using ModelDevolucao = ActiveControlApi.Models.Devolucao;

namespace ActiveControlApi.Services.Solicitacao
{
    public class SolicitacaoService : ISolicitacaoService
    {
        private readonly IUnitOfWork _uow;

        public SolicitacaoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        private IQueryable<ModelSolicitacao> GetQueryableWithIncludes()
        {
            return _uow.Solicitacao.GetQueryble()
                .Include(s => s.Usuario)
                .Include(s => s.UsuarioResponsavel)
                .Include(s => s.Ativo);
        }

        public async Task<IEnumerable<SolicitacaoDTO>> PegarTodos()
        {
            var solicitacoes = await GetQueryableWithIncludes().ToListAsync();
            if (!solicitacoes.Any())
                return Enumerable.Empty<SolicitacaoDTO>();

            return solicitacoes.ParaListaSolicitacaoDto();
        }

        public async Task<SolicitacaoDTO> PegarPorId(int idSolicitacao)
        {
            var solicitacao = await GetQueryableWithIncludes()
                .FirstOrDefaultAsync(s => s.Id == idSolicitacao);
            if (solicitacao == null)
                throw new KeyNotFoundException($"Solicitação com id {idSolicitacao} não encontrada.");

            return solicitacao.ParaSolicitacaoDto() ?? throw new InvalidOperationException("Erro ao converter solicitação.");
        }

        public async Task<SolicitacaoDTO> CriarSolicitacao(SolicitacaoDTO solicitacaoRegistro)
        {
            var entity = solicitacaoRegistro.ParaSolicitacao();
            if (entity == null)
                throw new ArgumentNullException(nameof(solicitacaoRegistro), "Não foi possível criar a solicitação.");

            // Validações
            if (string.IsNullOrWhiteSpace(entity.Titulo))
                throw new ArgumentException("Título da solicitação é obrigatório.");

            if (entity.StatusSolicitacao == null)
                entity.StatusSolicitacao = StatusSolicitacao.ABERTA;

            // Criar a solicitação primeiro
            var criado = _uow.Solicitacao.Create(entity);
            await _uow.CommitAsync(); // Salvar para obter o ID da solicitação

            // Criar registro relacionado baseado no tipo de solicitação
            switch (criado.TipoSolicitacao)
            {
                case TipoSolicitacao.Incidente:
                    var incidente = new ModelIncidente
                    {
                        SolicitacaoId = criado.Id,
                        Descricao = criado.Descricao,
                        Severidade = SeveridadeIncidente.Media // Valor padrão, pode ser ajustado
                    };
                    _uow.Incidente.Create(incidente);
                    break;

                case TipoSolicitacao.ManutencaoCorretiva:
                    var manutencaoCorretiva = new ModelManutencao
                    {
                        SolicitacaoId = criado.Id,
                        TipoManutencao = TipoManutencao.Corretiva,
                        DataCriacao = DateTime.UtcNow
                    };
                    _uow.Manutencao.Create(manutencaoCorretiva);
                    break;

                case TipoSolicitacao.ManutencaoPreventiva:
                    var manutencaoPreventiva = new ModelManutencao
                    {
                        SolicitacaoId = criado.Id,
                        TipoManutencao = TipoManutencao.Preventiva,
                        DataCriacao = DateTime.UtcNow
                    };
                    _uow.Manutencao.Create(manutencaoPreventiva);
                    break;

                case TipoSolicitacao.Devolução:
                    var devolucao = new ModelDevolucao
                    {
                        SolicitacaoId = criado.Id,
                        MotivoDevolucao = criado.Descricao // Usar a descrição da solicitação como motivo
                    };
                    _uow.Devolucao.Create(devolucao);
                    break;

                case TipoSolicitacao.AquisicaoEquipamento:
                    // Para aquisição de equipamento, não é necessário criar registro relacionado
                    break;

                default:
                    throw new InvalidOperationException($"Tipo de solicitação inválido: {criado.TipoSolicitacao}");
            }

            // Salvar os registros relacionados
            await _uow.CommitAsync();

            return criado.ParaSolicitacaoDto() ?? throw new InvalidOperationException("Erro ao criar solicitação.");
        }

        public async Task<SolicitacaoDTO> AtualizarSolicitacao(int idSolicitacao, SolicitacaoDTO solicitacaoRegistro)
        {
            var entity = await _uow.Solicitacao.Get(s => s.Id == idSolicitacao);
            if (entity == null)
                throw new KeyNotFoundException($"Solicitação com id {idSolicitacao} não encontrada.");

            if (!string.IsNullOrWhiteSpace(solicitacaoRegistro.Titulo))
                entity.Titulo = solicitacaoRegistro.Titulo;
            if (solicitacaoRegistro.UsuarioSolicitanteId > 0)
                entity.UsuarioSolicitanteId = solicitacaoRegistro.UsuarioSolicitanteId;
            if (solicitacaoRegistro.UsuarioResponsavelId.HasValue)
                entity.UsuarioResponsavelId = solicitacaoRegistro.UsuarioResponsavelId;
            if (solicitacaoRegistro.AtivoId > 0)
                entity.AtivoId = solicitacaoRegistro.AtivoId;
            if (!string.IsNullOrWhiteSpace(solicitacaoRegistro.Descricao))
                entity.Descricao = solicitacaoRegistro.Descricao;
            if (solicitacaoRegistro.TipoSolicitacao != default)
                entity.TipoSolicitacao = solicitacaoRegistro.TipoSolicitacao;
            if (solicitacaoRegistro.StatusSolicitacao.HasValue)
                entity.StatusSolicitacao = solicitacaoRegistro.StatusSolicitacao.Value;
            if (solicitacaoRegistro.Prioridade != default)
                entity.Prioridade = solicitacaoRegistro.Prioridade;
            if (solicitacaoRegistro.DataPrazo.HasValue)
                entity.DataPrazo = solicitacaoRegistro.DataPrazo;
            if (solicitacaoRegistro.DataFechamento.HasValue)
                entity.DataFechamento = solicitacaoRegistro.DataFechamento.Value;

            _uow.Solicitacao.Update(entity);
            await _uow.CommitAsync();

            return entity.ParaSolicitacaoDto() ?? throw new InvalidOperationException("Erro ao atualizar solicitação.");
        }

        public async Task<bool> RemoverSolicitacao(int idSolicitacao)
        {
            var entity = await _uow.Solicitacao.Get(s => s.Id == idSolicitacao);
            if (entity == null)
                throw new KeyNotFoundException($"Solicitação com id {idSolicitacao} não encontrada.");

            _uow.Solicitacao.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }

        // Filtros avançados
        public async Task<IEnumerable<SolicitacaoDTO>> BuscarPorStatus(StatusSolicitacao status)
        {
            var solicitacoes = await GetQueryableWithIncludes()
                .Where(s => s.StatusSolicitacao == status)
                .ToListAsync();
            return solicitacoes.ParaListaSolicitacaoDto();
        }

        public async Task<IEnumerable<SolicitacaoDTO>> BuscarPorTipo(TipoSolicitacao tipo)
        {
            var solicitacoes = await GetQueryableWithIncludes()
                .Where(s => s.TipoSolicitacao == tipo)
                .ToListAsync();
            return solicitacoes.ParaListaSolicitacaoDto();
        }

        public async Task<IEnumerable<SolicitacaoDTO>> BuscarPorPrioridade(PrioridadeSolicitacao prioridade)
        {
            var solicitacoes = await GetQueryableWithIncludes()
                .Where(s => s.Prioridade == prioridade)
                .ToListAsync();
            return solicitacoes.ParaListaSolicitacaoDto();
        }

        public async Task<IEnumerable<SolicitacaoDTO>> BuscarPorUsuarioSolicitante(int usuarioId)
        {
            var solicitacoes = await GetQueryableWithIncludes()
                .Where(s => s.UsuarioSolicitanteId == usuarioId)
                .ToListAsync();
            return solicitacoes.ParaListaSolicitacaoDto();
        }

        public async Task<IEnumerable<SolicitacaoDTO>> BuscarPorUsuarioResponsavel(int usuarioId)
        {
            var solicitacoes = await GetQueryableWithIncludes()
                .Where(s => s.UsuarioResponsavelId == usuarioId)
                .ToListAsync();
            return solicitacoes.ParaListaSolicitacaoDto();
        }

        public async Task<IEnumerable<SolicitacaoDTO>> BuscarPorAtivo(int ativoId)
        {
            var solicitacoes = await GetQueryableWithIncludes()
                .Where(s => s.AtivoId == ativoId)
                .ToListAsync();
            return solicitacoes.ParaListaSolicitacaoDto();
        }

        public async Task<IEnumerable<SolicitacaoDTO>> BuscarAtrasadas()
        {
            var hoje = DateTime.UtcNow;
            var solicitacoes = await GetQueryableWithIncludes()
                .Where(s => s.DataPrazo.HasValue &&
                           s.DataPrazo.Value < hoje &&
                           s.StatusSolicitacao != StatusSolicitacao.FINALIZADA &&
                           s.StatusSolicitacao != StatusSolicitacao.CANCELADA)
                .OrderBy(s => s.DataPrazo)
                .ToListAsync();
            return solicitacoes.ParaListaSolicitacaoDto();
        }

        public async Task<IEnumerable<SolicitacaoDTO>> BuscarPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var solicitacoes = await GetQueryableWithIncludes()
                .Where(s => s.DataAbertura >= dataInicio && s.DataAbertura <= dataFim)
                .ToListAsync();
            return solicitacoes.ParaListaSolicitacaoDto();
        }

        // Workflow
        public async Task<SolicitacaoDTO> AtribuirResponsavel(int solicitacaoId, int usuarioResponsavelId)
        {
            var solicitacao = await _uow.Solicitacao.Get(s => s.Id == solicitacaoId);
            if (solicitacao == null)
                throw new KeyNotFoundException($"Solicitação com id {solicitacaoId} não encontrada.");

            var usuario = await _uow.Usuario.Get(u => u.Id == usuarioResponsavelId);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuário com id {usuarioResponsavelId} não encontrado.");

            solicitacao.UsuarioResponsavelId = usuarioResponsavelId;
            if (solicitacao.StatusSolicitacao == null || solicitacao.StatusSolicitacao == StatusSolicitacao.ABERTA)
                solicitacao.StatusSolicitacao = StatusSolicitacao.EM_ANDAMENTO;

            _uow.Solicitacao.Update(solicitacao);
            await _uow.CommitAsync();

            return (await PegarPorId(solicitacaoId));
        }

        public async Task<SolicitacaoDTO> IniciarAtendimento(int solicitacaoId)
        {
            var solicitacao = await _uow.Solicitacao.Get(s => s.Id == solicitacaoId);
            if (solicitacao == null)
                throw new KeyNotFoundException($"Solicitação com id {solicitacaoId} não encontrada.");

            if (solicitacao.StatusSolicitacao == StatusSolicitacao.FINALIZADA)
                throw new InvalidOperationException("Não é possível iniciar atendimento de uma solicitação finalizada.");

            if (solicitacao.StatusSolicitacao == StatusSolicitacao.CANCELADA)
                throw new InvalidOperationException("Não é possível iniciar atendimento de uma solicitação cancelada.");

            solicitacao.StatusSolicitacao = StatusSolicitacao.EM_ANDAMENTO;
            _uow.Solicitacao.Update(solicitacao);
            await _uow.CommitAsync();

            return (await PegarPorId(solicitacaoId));
        }

        public async Task<SolicitacaoDTO> Finalizar(int solicitacaoId, string? observacao = null)
        {
            var solicitacao = await _uow.Solicitacao.Get(s => s.Id == solicitacaoId);
            if (solicitacao == null)
                throw new KeyNotFoundException($"Solicitação com id {solicitacaoId} não encontrada.");

            if (solicitacao.StatusSolicitacao == StatusSolicitacao.FINALIZADA)
                throw new InvalidOperationException("Solicitação já está finalizada.");

            if (solicitacao.StatusSolicitacao == StatusSolicitacao.CANCELADA)
                throw new InvalidOperationException("Não é possível finalizar uma solicitação cancelada.");

            solicitacao.StatusSolicitacao = StatusSolicitacao.FINALIZADA;
            solicitacao.DataFechamento = DateTime.UtcNow;

            // Se houver observação, pode ser adicionada como comentário (implementar depois)
            if (!string.IsNullOrWhiteSpace(observacao))
            {
                // TODO: Adicionar comentário automaticamente
            }

            _uow.Solicitacao.Update(solicitacao);
            await _uow.CommitAsync();

            return (await PegarPorId(solicitacaoId));
        }

        public async Task<SolicitacaoDTO> Cancelar(int solicitacaoId, string? motivo = null)
        {
            var solicitacao = await _uow.Solicitacao.Get(s => s.Id == solicitacaoId);
            if (solicitacao == null)
                throw new KeyNotFoundException($"Solicitação com id {solicitacaoId} não encontrada.");

            if (solicitacao.StatusSolicitacao == StatusSolicitacao.FINALIZADA)
                throw new InvalidOperationException("Não é possível cancelar uma solicitação finalizada.");

            if (solicitacao.StatusSolicitacao == StatusSolicitacao.CANCELADA)
                throw new InvalidOperationException("Solicitação já está cancelada.");

            solicitacao.StatusSolicitacao = StatusSolicitacao.CANCELADA;
            solicitacao.DataFechamento = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                solicitacao.Descricao += $"\n\n[MOTIVO DO CANCELAMENTO: {motivo}]";
            }

            _uow.Solicitacao.Update(solicitacao);
            await _uow.CommitAsync();

            return (await PegarPorId(solicitacaoId));
        }

        public async Task<SolicitacaoDTO> AlterarPrioridade(int solicitacaoId, PrioridadeSolicitacao prioridade)
        {
            var solicitacao = await _uow.Solicitacao.Get(s => s.Id == solicitacaoId);
            if (solicitacao == null)
                throw new KeyNotFoundException($"Solicitação com id {solicitacaoId} não encontrada.");

            solicitacao.Prioridade = prioridade;
            _uow.Solicitacao.Update(solicitacao);
            await _uow.CommitAsync();

            return (await PegarPorId(solicitacaoId));
        }

        // Relatórios e estatísticas
        public async Task<Dictionary<string, object>> ObterEstatisticas()
        {
            var todas = await GetQueryableWithIncludes().ToListAsync();
            var total = todas.Count;
            var abertas = todas.Count(s => s.StatusSolicitacao == StatusSolicitacao.ABERTA);
            var emAndamento = todas.Count(s => s.StatusSolicitacao == StatusSolicitacao.EM_ANDAMENTO);
            var finalizadas = todas.Count(s => s.StatusSolicitacao == StatusSolicitacao.FINALIZADA);
            var canceladas = todas.Count(s => s.StatusSolicitacao == StatusSolicitacao.CANCELADA);
            var atrasadas = todas.Count(s => s.DataPrazo.HasValue &&
                                            s.DataPrazo.Value < DateTime.UtcNow &&
                                            s.StatusSolicitacao != StatusSolicitacao.FINALIZADA &&
                                            s.StatusSolicitacao != StatusSolicitacao.CANCELADA);

            var porTipo = todas.GroupBy(s => s.TipoSolicitacao)
                .Select(g => new { Tipo = g.Key.ToString(), Quantidade = g.Count() })
                .ToList();

            var porPrioridade = todas.GroupBy(s => s.Prioridade)
                .Select(g => new { Prioridade = g.Key.ToString(), Quantidade = g.Count() })
                .ToList();

            var tempoMedioResolucao = todas
                .Where(s => s.StatusSolicitacao == StatusSolicitacao.FINALIZADA && s.DataFechamento.HasValue)
                .Select(s => (s.DataFechamento!.Value - s.DataAbertura).TotalDays)
                .DefaultIfEmpty(0)
                .Average();

            return new Dictionary<string, object>
            {
                { "Total", total },
                { "Abertas", abertas },
                { "EmAndamento", emAndamento },
                { "Finalizadas", finalizadas },
                { "Canceladas", canceladas },
                { "Atrasadas", atrasadas },
                { "PorTipo", porTipo },
                { "PorPrioridade", porPrioridade },
                { "TempoMedioResolucaoDias", Math.Round(tempoMedioResolucao, 2) }
            };
        }

        public async Task<Dictionary<string, object>> ObterEstatisticasPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var todas = await GetQueryableWithIncludes()
                .Where(s => s.DataAbertura >= dataInicio && s.DataAbertura <= dataFim)
                .ToListAsync();

            return new Dictionary<string, object>
            {
                { "Total", todas.Count },
                { "Abertas", todas.Count(s => s.StatusSolicitacao == StatusSolicitacao.ABERTA) },
                { "EmAndamento", todas.Count(s => s.StatusSolicitacao == StatusSolicitacao.EM_ANDAMENTO) },
                { "Finalizadas", todas.Count(s => s.StatusSolicitacao == StatusSolicitacao.FINALIZADA) },
                { "Canceladas", todas.Count(s => s.StatusSolicitacao == StatusSolicitacao.CANCELADA) },
                { "Atrasadas", todas.Count(s => s.DataPrazo.HasValue && s.DataPrazo.Value < DateTime.UtcNow) }
            };
        }

        public async Task<IEnumerable<SolicitacaoDTO>> BuscarMinhasSolicitacoes(int usuarioId)
        {
            return await BuscarPorUsuarioSolicitante(usuarioId);
        }

        public async Task<IEnumerable<SolicitacaoDTO>> BuscarSolicitacoesAtribuidas(int usuarioId)
        {
            return await BuscarPorUsuarioResponsavel(usuarioId);
        }
    }
}

