using ActiveControlApi.DTO.Incidente;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;
using ModelIncidente = ActiveControlApi.Models.Incidente;

namespace ActiveControlApi.Services.Incidente
{
    public class IncidenteService : IIncidenteService
    {
        private readonly IUnitOfWork _uow;

        public IncidenteService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        private IQueryable<ModelIncidente> GetQueryableWithIncludes()
        {
            return _uow.Incidente.GetQueryble()
                .Include(i => i.Solicitacao)
                    .ThenInclude(s => s.Ativo)
                .Include(i => i.UsuarioResponsavel);
        }

        public async Task<IEnumerable<IncidenteDTO>> PegarTodos()
        {
            var incidentes = await GetQueryableWithIncludes().ToListAsync();
            if (!incidentes.Any())
                return Enumerable.Empty<IncidenteDTO>();

            return incidentes.ParaListaIncidenteDto();
        }

        public async Task<IncidenteDTO> PegarPorId(int idIncidente)
        {
            var incidente = await GetQueryableWithIncludes()
                .FirstOrDefaultAsync(i => i.Id == idIncidente);
            if (incidente == null)
                throw new KeyNotFoundException($"Incidente com id {idIncidente} não encontrado.");

            return incidente.ParaIncidenteDto() ?? throw new InvalidOperationException("Erro ao converter incidente.");
        }

        public async Task<IncidenteDTO> CriarIncidente(IncidenteDTO incidenteRegistro)
        {
            var entity = incidenteRegistro.ParaIncidente();
            if (entity == null)
                throw new ArgumentNullException(nameof(incidenteRegistro), "Não foi possível criar o incidente.");

            var criado = _uow.Incidente.Create(entity);
            await _uow.CommitAsync();

            return criado.ParaIncidenteDto() ?? throw new InvalidOperationException("Erro ao criar incidente.");
        }

        public async Task<IncidenteDTO> AtualizarIncidente(int idIncidente, IncidenteDTO incidenteRegistro)
        {
            var entity = await _uow.Incidente.Get(i => i.Id == idIncidente);
            if (entity == null)
                throw new KeyNotFoundException($"Incidente com id {idIncidente} não encontrado.");

            if (incidenteRegistro.SolicitacaoId > 0)
                entity.SolicitacaoId = incidenteRegistro.SolicitacaoId;
            if (incidenteRegistro.Severidade != default)
                entity.Severidade = incidenteRegistro.Severidade;
            if (incidenteRegistro.StatusIncidente != default)
                entity.StatusIncidente = incidenteRegistro.StatusIncidente;
            if (incidenteRegistro.UsuarioResponsavelId.HasValue)
                entity.UsuarioResponsavelId = incidenteRegistro.UsuarioResponsavelId;
            if (incidenteRegistro.Prioridade != default)
                entity.Prioridade = incidenteRegistro.Prioridade;
            if (!string.IsNullOrWhiteSpace(incidenteRegistro.Descricao))
                entity.Descricao = incidenteRegistro.Descricao;
            if (incidenteRegistro.DataPrazo.HasValue)
                entity.DataPrazo = incidenteRegistro.DataPrazo;
            if (!string.IsNullOrWhiteSpace(incidenteRegistro.SolucaoAplicada))
                entity.SolucaoAplicada = incidenteRegistro.SolucaoAplicada;
            if (!string.IsNullOrWhiteSpace(incidenteRegistro.CausaRaiz))
                entity.CausaRaiz = incidenteRegistro.CausaRaiz;

            _uow.Incidente.Update(entity);
            await _uow.CommitAsync();

            return entity.ParaIncidenteDto() ?? throw new InvalidOperationException("Erro ao atualizar incidente.");
        }

        public async Task<bool> RemoverIncidente(int idIncidente)
        {
            var entity = await _uow.Incidente.Get(i => i.Id == idIncidente);
            if (entity == null)
                throw new KeyNotFoundException($"Incidente com id {idIncidente} não encontrado.");

            _uow.Incidente.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }

        // Filtros avançados
        public async Task<IEnumerable<IncidenteDTO>> BuscarPorStatus(StatusIncidente status)
        {
            var incidentes = await GetQueryableWithIncludes()
                .Where(i => i.StatusIncidente == status)
                .ToListAsync();
            return incidentes.ParaListaIncidenteDto();
        }

        public async Task<IEnumerable<IncidenteDTO>> BuscarPorSeveridade(SeveridadeIncidente severidade)
        {
            var incidentes = await GetQueryableWithIncludes()
                .Where(i => i.Severidade == severidade)
                .ToListAsync();
            return incidentes.ParaListaIncidenteDto();
        }

        public async Task<IEnumerable<IncidenteDTO>> BuscarPorPrioridade(PrioridadeSolicitacao prioridade)
        {
            var incidentes = await GetQueryableWithIncludes()
                .Where(i => i.Prioridade == prioridade)
                .ToListAsync();
            return incidentes.ParaListaIncidenteDto();
        }

        public async Task<IEnumerable<IncidenteDTO>> BuscarPorUsuarioResponsavel(int usuarioId)
        {
            var incidentes = await GetQueryableWithIncludes()
                .Where(i => i.UsuarioResponsavelId == usuarioId)
                .ToListAsync();
            return incidentes.ParaListaIncidenteDto();
        }

        public async Task<IEnumerable<IncidenteDTO>> BuscarAbertos()
        {
            var incidentes = await GetQueryableWithIncludes()
                .Where(i => i.StatusIncidente == StatusIncidente.Aberto)
                .OrderByDescending(i => i.DataAbertura)
                .ToListAsync();
            return incidentes.ParaListaIncidenteDto();
        }

        public async Task<IEnumerable<IncidenteDTO>> BuscarAtrasados()
        {
            var hoje = DateTime.UtcNow;
            var incidentes = await GetQueryableWithIncludes()
                .Where(i => i.DataPrazo.HasValue &&
                         i.DataPrazo.Value < hoje &&
                         i.StatusIncidente != StatusIncidente.Resolvido &&
                         i.StatusIncidente != StatusIncidente.Cancelado)
                .OrderBy(i => i.DataPrazo)
                .ToListAsync();
            return incidentes.ParaListaIncidenteDto();
        }

        public async Task<IEnumerable<IncidenteDTO>> BuscarPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var incidentes = await GetQueryableWithIncludes()
                .Where(i => i.DataAbertura >= dataInicio && i.DataAbertura <= dataFim)
                .ToListAsync();
            return incidentes.ParaListaIncidenteDto();
        }

        // Workflow
        public async Task<IncidenteDTO> AtribuirResponsavel(int incidenteId, int usuarioResponsavelId)
        {
            var incidente = await _uow.Incidente.Get(i => i.Id == incidenteId);
            if (incidente == null)
                throw new KeyNotFoundException($"Incidente com id {incidenteId} não encontrado.");

            var usuario = await _uow.Usuario.Get(u => u.Id == usuarioResponsavelId);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuário com id {usuarioResponsavelId} não encontrado.");

            incidente.UsuarioResponsavelId = usuarioResponsavelId;
            if (incidente.StatusIncidente == StatusIncidente.Aberto)
                incidente.StatusIncidente = StatusIncidente.EmAnalise;

            _uow.Incidente.Update(incidente);
            await _uow.CommitAsync();

            return await PegarPorId(incidenteId);
        }

        public async Task<IncidenteDTO> IniciarAnalise(int incidenteId)
        {
            var incidente = await _uow.Incidente.Get(i => i.Id == incidenteId);
            if (incidente == null)
                throw new KeyNotFoundException($"Incidente com id {incidenteId} não encontrado.");

            if (incidente.StatusIncidente == StatusIncidente.Resolvido)
                throw new InvalidOperationException("Incidente já está resolvido.");

            if (incidente.StatusIncidente == StatusIncidente.Cancelado)
                throw new InvalidOperationException("Não é possível iniciar análise de um incidente cancelado.");

            incidente.StatusIncidente = StatusIncidente.EmAnalise;

            _uow.Incidente.Update(incidente);
            await _uow.CommitAsync();

            return await PegarPorId(incidenteId);
        }

        public async Task<IncidenteDTO> IniciarResolucao(int incidenteId)
        {
            var incidente = await _uow.Incidente.Get(i => i.Id == incidenteId);
            if (incidente == null)
                throw new KeyNotFoundException($"Incidente com id {incidenteId} não encontrado.");

            if (incidente.StatusIncidente == StatusIncidente.Resolvido)
                throw new InvalidOperationException("Incidente já está resolvido.");

            if (incidente.StatusIncidente == StatusIncidente.Cancelado)
                throw new InvalidOperationException("Não é possível iniciar resolução de um incidente cancelado.");

            incidente.StatusIncidente = StatusIncidente.EmResolucao;
            incidente.DataInicioResolucao = DateTime.UtcNow;

            _uow.Incidente.Update(incidente);
            await _uow.CommitAsync();

            return await PegarPorId(incidenteId);
        }

        public async Task<IncidenteDTO> Resolver(int incidenteId, string? solucaoAplicada = null, string? causaRaiz = null)
        {
            var incidente = await _uow.Incidente.Get(i => i.Id == incidenteId);
            if (incidente == null)
                throw new KeyNotFoundException($"Incidente com id {incidenteId} não encontrado.");

            if (incidente.StatusIncidente == StatusIncidente.Resolvido)
                throw new InvalidOperationException("Incidente já está resolvido.");

            if (incidente.StatusIncidente == StatusIncidente.Cancelado)
                throw new InvalidOperationException("Não é possível resolver um incidente cancelado.");

            incidente.StatusIncidente = StatusIncidente.Resolvido;
            incidente.DataResolucao = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(solucaoAplicada))
                incidente.SolucaoAplicada = solucaoAplicada;

            if (!string.IsNullOrWhiteSpace(causaRaiz))
                incidente.CausaRaiz = causaRaiz;

            _uow.Incidente.Update(incidente);
            await _uow.CommitAsync();

            return await PegarPorId(incidenteId);
        }

        public async Task<IncidenteDTO> Cancelar(int incidenteId, string? motivo = null)
        {
            var incidente = await _uow.Incidente.Get(i => i.Id == incidenteId);
            if (incidente == null)
                throw new KeyNotFoundException($"Incidente com id {incidenteId} não encontrado.");

            if (incidente.StatusIncidente == StatusIncidente.Resolvido)
                throw new InvalidOperationException("Não é possível cancelar um incidente resolvido.");

            if (incidente.StatusIncidente == StatusIncidente.Cancelado)
                throw new InvalidOperationException("Incidente já está cancelado.");

            incidente.StatusIncidente = StatusIncidente.Cancelado;

            if (!string.IsNullOrWhiteSpace(motivo))
                incidente.Descricao += $"\n\n[MOTIVO DO CANCELAMENTO: {motivo}]";

            _uow.Incidente.Update(incidente);
            await _uow.CommitAsync();

            return await PegarPorId(incidenteId);
        }

        public async Task<IncidenteDTO> AlterarPrioridade(int incidenteId, PrioridadeSolicitacao prioridade)
        {
            var incidente = await _uow.Incidente.Get(i => i.Id == incidenteId);
            if (incidente == null)
                throw new KeyNotFoundException($"Incidente com id {incidenteId} não encontrado.");

            incidente.Prioridade = prioridade;
            _uow.Incidente.Update(incidente);
            await _uow.CommitAsync();

            return await PegarPorId(incidenteId);
        }

        public async Task<IncidenteDTO> AlterarSeveridade(int incidenteId, SeveridadeIncidente severidade)
        {
            var incidente = await _uow.Incidente.Get(i => i.Id == incidenteId);
            if (incidente == null)
                throw new KeyNotFoundException($"Incidente com id {incidenteId} não encontrado.");

            incidente.Severidade = severidade;
            _uow.Incidente.Update(incidente);
            await _uow.CommitAsync();

            return await PegarPorId(incidenteId);
        }

        // Relatórios
        public async Task<Dictionary<string, object>> ObterEstatisticas()
        {
            var todos = await GetQueryableWithIncludes().ToListAsync();
            var total = todos.Count;
            var abertos = todos.Count(i => i.StatusIncidente == StatusIncidente.Aberto);
            var emAnalise = todos.Count(i => i.StatusIncidente == StatusIncidente.EmAnalise);
            var emResolucao = todos.Count(i => i.StatusIncidente == StatusIncidente.EmResolucao);
            var resolvidos = todos.Count(i => i.StatusIncidente == StatusIncidente.Resolvido);
            var cancelados = todos.Count(i => i.StatusIncidente == StatusIncidente.Cancelado);
            var atrasados = todos.Count(i => i.DataPrazo.HasValue &&
                                            i.DataPrazo.Value < DateTime.UtcNow &&
                                            i.StatusIncidente != StatusIncidente.Resolvido &&
                                            i.StatusIncidente != StatusIncidente.Cancelado);

            var porSeveridade = todos.GroupBy(i => i.Severidade)
                .Select(g => new { Severidade = g.Key.ToString(), Quantidade = g.Count() })
                .ToList();

            var porPrioridade = todos.GroupBy(i => i.Prioridade)
                .Select(g => new { Prioridade = g.Key.ToString(), Quantidade = g.Count() })
                .ToList();

            var tempoMedioResolucao = todos
                .Where(i => i.StatusIncidente == StatusIncidente.Resolvido && 
                           i.DataResolucao.HasValue && 
                           i.DataInicioResolucao.HasValue)
                .Select(i => (i.DataResolucao!.Value - i.DataInicioResolucao!.Value).TotalHours)
                .DefaultIfEmpty(0)
                .Average();

            return new Dictionary<string, object>
            {
                { "Total", total },
                { "Abertos", abertos },
                { "EmAnalise", emAnalise },
                { "EmResolucao", emResolucao },
                { "Resolvidos", resolvidos },
                { "Cancelados", cancelados },
                { "Atrasados", atrasados },
                { "PorSeveridade", porSeveridade },
                { "PorPrioridade", porPrioridade },
                { "TempoMedioResolucaoHoras", Math.Round(tempoMedioResolucao, 2) }
            };
        }

        public async Task<IEnumerable<IncidenteDTO>> BuscarIncidentesAtribuidos(int usuarioId)
        {
            return await BuscarPorUsuarioResponsavel(usuarioId);
        }
    }
}

