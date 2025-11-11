using ActiveControlApi.DTO.Manutencao;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;
using ModelManutencao = ActiveControlApi.Models.Manutencao;

namespace ActiveControlApi.Services.Manutencao
{
    public class ManutencaoService : IManutencaoService
    {
        private readonly IUnitOfWork _uow;

        public ManutencaoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        private IQueryable<ModelManutencao> GetQueryableWithIncludes()
        {
            return _uow.Manutencao.GetQueryble()
                .Include(m => m.Solicitacao)
                    .ThenInclude(s => s.Ativo)
                .Include(m => m.UsuarioResponsavel);
        }

        public async Task<IEnumerable<ManutencaoDTO>> PegarTodos()
        {
            var manutencoes = await GetQueryableWithIncludes().ToListAsync();
            if (!manutencoes.Any())
                return Enumerable.Empty<ManutencaoDTO>();

            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<ManutencaoDTO> PegarPorId(int idManutencao)
        {
            var manutencao = await GetQueryableWithIncludes()
                .FirstOrDefaultAsync(m => m.Id == idManutencao);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {idManutencao} não encontrada.");

            return manutencao.ParaManutencaoDto() ?? throw new InvalidOperationException("Erro ao converter manutenção.");
        }

        public async Task<ManutencaoDTO> CriarManutencao(ManutencaoDTO manutencaoRegistro)
        {
            var entity = manutencaoRegistro.ParaManutencao();
            if (entity == null)
                throw new ArgumentNullException(nameof(manutencaoRegistro), "Não foi possível criar a manutenção.");

            var criado = _uow.Manutencao.Create(entity);
            await _uow.CommitAsync();

            return criado.ParaManutencaoDto() ?? throw new InvalidOperationException("Erro ao criar manutenção.");
        }

        public async Task<ManutencaoDTO> AtualizarManutencao(int idManutencao, ManutencaoDTO manutencaoRegistro)
        {
            var entity = await _uow.Manutencao.Get(m => m.Id == idManutencao);
            if (entity == null)
                throw new KeyNotFoundException($"Manutenção com id {idManutencao} não encontrada.");

            if (manutencaoRegistro.SolicitacaoId > 0)
                entity.SolicitacaoId = manutencaoRegistro.SolicitacaoId;
            if (manutencaoRegistro.TipoManutencao != default)
                entity.TipoManutencao = manutencaoRegistro.TipoManutencao;
            if (manutencaoRegistro.StatusManutencao != default)
                entity.StatusManutencao = manutencaoRegistro.StatusManutencao;
            if (manutencaoRegistro.UsuarioResponsavelId.HasValue)
                entity.UsuarioResponsavelId = manutencaoRegistro.UsuarioResponsavelId;
            if (manutencaoRegistro.Prioridade != default)
                entity.Prioridade = manutencaoRegistro.Prioridade;
            if (manutencaoRegistro.DataAgendada.HasValue)
                entity.DataAgendada = manutencaoRegistro.DataAgendada;
            if (manutencaoRegistro.DataInicio.HasValue)
                entity.DataInicio = manutencaoRegistro.DataInicio;
            if (manutencaoRegistro.DataFechamento.HasValue)
                entity.DataFechamento = manutencaoRegistro.DataFechamento;
            if (manutencaoRegistro.DataPrazo.HasValue)
                entity.DataPrazo = manutencaoRegistro.DataPrazo;
            if (manutencaoRegistro.CustoEstimado.HasValue)
                entity.CustoEstimado = manutencaoRegistro.CustoEstimado;
            if (manutencaoRegistro.CustoReal.HasValue)
                entity.CustoReal = manutencaoRegistro.CustoReal;
            if (!string.IsNullOrWhiteSpace(manutencaoRegistro.Observacoes))
                entity.Observacoes = manutencaoRegistro.Observacoes;
            if (!string.IsNullOrWhiteSpace(manutencaoRegistro.SolucaoAplicada))
                entity.SolucaoAplicada = manutencaoRegistro.SolucaoAplicada;

            _uow.Manutencao.Update(entity);
            await _uow.CommitAsync();

            return entity.ParaManutencaoDto() ?? throw new InvalidOperationException("Erro ao atualizar manutenção.");
        }

        public async Task<bool> RemoverManutencao(int idManutencao)
        {
            var entity = await _uow.Manutencao.Get(m => m.Id == idManutencao);
            if (entity == null)
                throw new KeyNotFoundException($"Manutenção com id {idManutencao} não encontrada.");

            _uow.Manutencao.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }

        // Filtros avançados
        public async Task<IEnumerable<ManutencaoDTO>> BuscarPorStatus(StatusManutencao status)
        {
            var manutencoes = await GetQueryableWithIncludes()
                .Where(m => m.StatusManutencao == status)
                .ToListAsync();
            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarPorTipo(TipoManutencao tipo)
        {
            var manutencoes = await GetQueryableWithIncludes()
                .Where(m => m.TipoManutencao == tipo)
                .ToListAsync();
            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarPorPrioridade(PrioridadeSolicitacao prioridade)
        {
            var manutencoes = await GetQueryableWithIncludes()
                .Where(m => m.Prioridade == prioridade)
                .ToListAsync();
            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarPorUsuarioResponsavel(int usuarioId)
        {
            var manutencoes = await GetQueryableWithIncludes()
                .Where(m => m.UsuarioResponsavelId == usuarioId)
                .ToListAsync();
            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarAtrasadas()
        {
            var hoje = DateTime.UtcNow;
            var manutencoes = await GetQueryableWithIncludes()
                .Where(m => m.DataPrazo.HasValue &&
                           m.DataPrazo.Value < hoje &&
                           m.StatusManutencao != StatusManutencao.Concluida &&
                           m.StatusManutencao != StatusManutencao.Cancelada)
                .OrderBy(m => m.DataPrazo)
                .ToListAsync();
            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var manutencoes = await GetQueryableWithIncludes()
                .Where(m => m.DataCriacao >= dataInicio && m.DataCriacao <= dataFim)
                .ToListAsync();
            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarAgendadas()
        {
            var hoje = DateTime.UtcNow;
            var manutencoes = await GetQueryableWithIncludes()
                .Where(m => m.StatusManutencao == StatusManutencao.Agendada &&
                           m.DataAgendada.HasValue &&
                           m.DataAgendada.Value >= hoje)
                .OrderBy(m => m.DataAgendada)
                .ToListAsync();
            return manutencoes.ParaListaManutencaoDto();
        }

        // Workflow
        public async Task<ManutencaoDTO> AtribuirResponsavel(int manutencaoId, int usuarioResponsavelId)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == manutencaoId);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {manutencaoId} não encontrada.");

            var usuario = await _uow.Usuario.Get(u => u.Id == usuarioResponsavelId);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuário com id {usuarioResponsavelId} não encontrado.");

            manutencao.UsuarioResponsavelId = usuarioResponsavelId;
            if (manutencao.StatusManutencao == StatusManutencao.Agendada)
                manutencao.StatusManutencao = StatusManutencao.EmAndamento;

            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            return await PegarPorId(manutencaoId);
        }

        public async Task<ManutencaoDTO> Agendar(int manutencaoId, DateTime dataAgendada)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == manutencaoId);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {manutencaoId} não encontrada.");

            manutencao.DataAgendada = dataAgendada;
            manutencao.StatusManutencao = StatusManutencao.Agendada;

            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            return await PegarPorId(manutencaoId);
        }

        public async Task<ManutencaoDTO> Iniciar(int manutencaoId)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == manutencaoId);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {manutencaoId} não encontrada.");

            if (manutencao.StatusManutencao == StatusManutencao.Concluida)
                throw new InvalidOperationException("Manutenção já está concluída.");

            if (manutencao.StatusManutencao == StatusManutencao.Cancelada)
                throw new InvalidOperationException("Não é possível iniciar uma manutenção cancelada.");

            manutencao.StatusManutencao = StatusManutencao.EmAndamento;
            manutencao.DataInicio = DateTime.UtcNow;

            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            return await PegarPorId(manutencaoId);
        }

        public async Task<ManutencaoDTO> Concluir(int manutencaoId, string? solucaoAplicada = null, decimal? custoReal = null)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == manutencaoId);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {manutencaoId} não encontrada.");

            if (manutencao.StatusManutencao == StatusManutencao.Concluida)
                throw new InvalidOperationException("Manutenção já está concluída.");

            if (manutencao.StatusManutencao == StatusManutencao.Cancelada)
                throw new InvalidOperationException("Não é possível concluir uma manutenção cancelada.");

            manutencao.StatusManutencao = StatusManutencao.Concluida;
            manutencao.DataFechamento = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(solucaoAplicada))
                manutencao.SolucaoAplicada = solucaoAplicada;

            if (custoReal.HasValue)
                manutencao.CustoReal = custoReal;

            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            return await PegarPorId(manutencaoId);
        }

        public async Task<ManutencaoDTO> Cancelar(int manutencaoId, string? motivo = null)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == manutencaoId);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {manutencaoId} não encontrada.");

            if (manutencao.StatusManutencao == StatusManutencao.Concluida)
                throw new InvalidOperationException("Não é possível cancelar uma manutenção concluída.");

            if (manutencao.StatusManutencao == StatusManutencao.Cancelada)
                throw new InvalidOperationException("Manutenção já está cancelada.");

            manutencao.StatusManutencao = StatusManutencao.Cancelada;
            manutencao.DataFechamento = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(motivo))
                manutencao.Observacoes = $"Cancelada: {motivo}";

            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            return await PegarPorId(manutencaoId);
        }

        public async Task<ManutencaoDTO> AlterarPrioridade(int manutencaoId, PrioridadeSolicitacao prioridade)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == manutencaoId);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {manutencaoId} não encontrada.");

            manutencao.Prioridade = prioridade;
            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            return await PegarPorId(manutencaoId);
        }

        // Relatórios
        public async Task<Dictionary<string, object>> ObterEstatisticas()
        {
            var todas = await GetQueryableWithIncludes().ToListAsync();
            var total = todas.Count;
            var agendadas = todas.Count(m => m.StatusManutencao == StatusManutencao.Agendada);
            var emAndamento = todas.Count(m => m.StatusManutencao == StatusManutencao.EmAndamento);
            var concluidas = todas.Count(m => m.StatusManutencao == StatusManutencao.Concluida);
            var canceladas = todas.Count(m => m.StatusManutencao == StatusManutencao.Cancelada);
            var atrasadas = todas.Count(m => m.DataPrazo.HasValue &&
                                            m.DataPrazo.Value < DateTime.UtcNow &&
                                            m.StatusManutencao != StatusManutencao.Concluida &&
                                            m.StatusManutencao != StatusManutencao.Cancelada);

            var porTipo = todas.GroupBy(m => m.TipoManutencao)
                .Select(g => new { Tipo = g.Key.ToString(), Quantidade = g.Count() })
                .ToList();

            var custoTotalEstimado = todas.Where(m => m.CustoEstimado.HasValue).Sum(m => m.CustoEstimado!.Value);
            var custoTotalReal = todas.Where(m => m.CustoReal.HasValue).Sum(m => m.CustoReal!.Value);

            var tempoMedioResolucao = todas
                .Where(m => m.StatusManutencao == StatusManutencao.Concluida && m.DataFechamento.HasValue && m.DataInicio.HasValue)
                .Select(m => (m.DataFechamento!.Value - m.DataInicio!.Value).TotalHours)
                .DefaultIfEmpty(0)
                .Average();

            return new Dictionary<string, object>
            {
                { "Total", total },
                { "Agendadas", agendadas },
                { "EmAndamento", emAndamento },
                { "Concluidas", concluidas },
                { "Canceladas", canceladas },
                { "Atrasadas", atrasadas },
                { "PorTipo", porTipo },
                { "CustoTotalEstimado", custoTotalEstimado },
                { "CustoTotalReal", custoTotalReal },
                { "TempoMedioResolucaoHoras", Math.Round(tempoMedioResolucao, 2) }
            };
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarManutencoesAtribuidas(int usuarioId)
        {
            return await BuscarPorUsuarioResponsavel(usuarioId);
        }
    }
}

