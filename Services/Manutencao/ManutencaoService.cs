using ActiveControlApi.DTO.Manutencao;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Services.Manutencao
{
    public class ManutencaoService : IManutencaoService
    {
        private readonly IUnitOfWork _uow;

        public ManutencaoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        private IQueryable<Models.Manutencao> ObterQueryComIncludes()
        {
            return _uow.Manutencao.GetQueryble()
                .Include(m => m.Solicitacao)
                    .ThenInclude(s => s!.Ativo)
                .Include(m => m.UsuarioResponsavel);
        }

        private IQueryable<Models.Manutencao> AplicarFiltros(IQueryable<Models.Manutencao> query, FiltroManutencaoDTO? filtro)
        {
            if (filtro == null) return query;

            if (filtro.Status.HasValue)
                query = query.Where(m => m.StatusManutencao == filtro.Status.Value);

            if (filtro.Tipo.HasValue)
                query = query.Where(m => m.TipoManutencao == filtro.Tipo.Value);

            if (filtro.Prioridade.HasValue)
                query = query.Where(m => m.Prioridade == filtro.Prioridade.Value);

            if (filtro.AtivoId.HasValue)
                query = query.Where(m => m.Solicitacao != null && m.Solicitacao.AtivoId == filtro.AtivoId.Value);

            if (filtro.UsuarioResponsavelId.HasValue)
                query = query.Where(m => m.UsuarioResponsavelId == filtro.UsuarioResponsavelId.Value);

            if (filtro.SolicitacaoId.HasValue)
                query = query.Where(m => m.SolicitacaoId == filtro.SolicitacaoId.Value);

            if (filtro.DataInicio.HasValue)
                query = query.Where(m => m.DataCriacao >= filtro.DataInicio.Value);

            if (filtro.DataFim.HasValue)
                query = query.Where(m => m.DataCriacao <= filtro.DataFim.Value);

            if (filtro.Atrasadas == true)
            {
                var hoje = DateTime.UtcNow;
                query = query.Where(m => 
                    m.DataPrazo.HasValue && 
                    m.DataPrazo.Value < hoje &&
                    m.StatusManutencao != StatusManutencao.Concluida &&
                    m.StatusManutencao != StatusManutencao.Cancelada);
            }

            return query;
        }

        public async Task<ManutencaoDTO> Criar(CriarManutencaoDTO dto)
        {
            // Validar se a solicitação existe
            var solicitacao = await _uow.Solicitacao.Get(s => s.Id == dto.SolicitacaoId);
            if (solicitacao == null)
                throw new KeyNotFoundException($"Solicitação com id {dto.SolicitacaoId} não encontrada.");

            // Validar se já existe manutenção para esta solicitação
            var existeManutencao = await _uow.Manutencao.Any(m => m.SolicitacaoId == dto.SolicitacaoId);
            if (existeManutencao)
                throw new InvalidOperationException("Já existe uma manutenção para esta solicitação.");

            // Validar usuário responsável se informado
            if (dto.UsuarioResponsavelId.HasValue)
            {
                var usuario = await _uow.Usuario.Get(u => u.Id == dto.UsuarioResponsavelId.Value);
                if (usuario == null)
                    throw new KeyNotFoundException($"Usuário responsável com id {dto.UsuarioResponsavelId.Value} não encontrado.");
            }

            var manutencao = new Models.Manutencao
            {
                SolicitacaoId = dto.SolicitacaoId,
                TipoManutencao = dto.TipoManutencao,
                StatusManutencao = StatusManutencao.Agendada,
                UsuarioResponsavelId = dto.UsuarioResponsavelId,
                Prioridade = dto.Prioridade,
                DataCriacao = DateTime.UtcNow,
                DataAgendada = dto.DataAgendada,
                DataPrazo = dto.DataPrazo,
                CustoEstimado = dto.CustoEstimado,
                Observacoes = dto.Observacoes
            };

            var criada = _uow.Manutencao.Create(manutencao);
            await _uow.CommitAsync();

            // Buscar com includes para retornar dados completos
            var manutencaoCompleta = await ObterQueryComIncludes()
                .FirstOrDefaultAsync(m => m.Id == criada.Id);

            return manutencaoCompleta!.ParaManutencaoDto()!;
        }

        public async Task<ManutencaoDTO> PegarPorId(int id)
        {
            var manutencao = await ObterQueryComIncludes()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {id} não encontrada.");

            return manutencao.ParaManutencaoDto()!;
        }

        public async Task<IEnumerable<ManutencaoDTO>> PegarTodos()
        {
            var manutencoes = await ObterQueryComIncludes()
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<ManutencaoDTO> Atualizar(int id, AtualizarManutencaoDTO dto)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == id);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {id} não encontrada.");

            // Validar se pode atualizar (não pode atualizar se estiver concluída ou cancelada)
            if (manutencao.StatusManutencao == StatusManutencao.Concluida ||
                manutencao.StatusManutencao == StatusManutencao.Cancelada)
                throw new InvalidOperationException("Não é possível atualizar uma manutenção concluída ou cancelada.");

            // Validar usuário responsável se informado
            if (dto.UsuarioResponsavelId.HasValue)
            {
                var usuario = await _uow.Usuario.Get(u => u.Id == dto.UsuarioResponsavelId.Value);
                if (usuario == null)
                    throw new KeyNotFoundException($"Usuário responsável com id {dto.UsuarioResponsavelId.Value} não encontrado.");
                manutencao.UsuarioResponsavelId = dto.UsuarioResponsavelId.Value;
            }

            if (dto.Prioridade.HasValue)
                manutencao.Prioridade = dto.Prioridade.Value;

            if (dto.DataAgendada.HasValue)
                manutencao.DataAgendada = dto.DataAgendada.Value;

            if (dto.DataPrazo.HasValue)
                manutencao.DataPrazo = dto.DataPrazo.Value;

            if (dto.CustoEstimado.HasValue)
                manutencao.CustoEstimado = dto.CustoEstimado.Value;

            if (dto.CustoReal.HasValue)
                manutencao.CustoReal = dto.CustoReal.Value;

            if (!string.IsNullOrWhiteSpace(dto.Observacoes))
                manutencao.Observacoes = dto.Observacoes;

            if (!string.IsNullOrWhiteSpace(dto.SolucaoAplicada))
                manutencao.SolucaoAplicada = dto.SolucaoAplicada;

            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            // Buscar com includes para retornar dados completos
            var manutencaoCompleta = await ObterQueryComIncludes()
                .FirstOrDefaultAsync(m => m.Id == id);

            return manutencaoCompleta!.ParaManutencaoDto()!;
        }

        public async Task<bool> Remover(int id)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == id);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {id} não encontrada.");

            // Validar se pode remover (não pode remover se estiver em andamento ou concluída)
            if (manutencao.StatusManutencao == StatusManutencao.EmAndamento ||
                manutencao.StatusManutencao == StatusManutencao.Concluida)
                throw new InvalidOperationException("Não é possível remover uma manutenção em andamento ou concluída.");

            _uow.Manutencao.Delete(manutencao);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }

        public async Task<(IEnumerable<ManutencaoDTO> itens, int total)> PegarPaginado(int pagina, int tamanhoPagina, FiltroManutencaoDTO? filtro = null)
        {
            pagina = pagina <= 0 ? 1 : pagina;
            tamanhoPagina = tamanhoPagina <= 0 ? 10 : Math.Min(tamanhoPagina, 100);

            var query = ObterQueryComIncludes();
            query = AplicarFiltros(query, filtro);

            var total = await query.CountAsync();
            var itens = await query
                .OrderByDescending(m => m.DataCriacao)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (itens.ParaListaManutencaoDto(), total);
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarComFiltros(FiltroManutencaoDTO filtro)
        {
            var query = ObterQueryComIncludes();
            query = AplicarFiltros(query, filtro);

            var manutencoes = await query
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<ManutencaoDTO> IniciarManutencao(int id, int? usuarioResponsavelId = null)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == id);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {id} não encontrada.");

            if (manutencao.StatusManutencao != StatusManutencao.Agendada)
                throw new InvalidOperationException("Apenas manutenções agendadas podem ser iniciadas.");

            if (usuarioResponsavelId.HasValue)
            {
                var usuario = await _uow.Usuario.Get(u => u.Id == usuarioResponsavelId.Value);
                if (usuario == null)
                    throw new KeyNotFoundException($"Usuário responsável com id {usuarioResponsavelId.Value} não encontrado.");
                manutencao.UsuarioResponsavelId = usuarioResponsavelId.Value;
            }
            else if (!manutencao.UsuarioResponsavelId.HasValue)
                throw new InvalidOperationException("É necessário informar um usuário responsável para iniciar a manutenção.");

            manutencao.StatusManutencao = StatusManutencao.EmAndamento;
            manutencao.DataInicio = DateTime.UtcNow;

            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            // Atualizar status do ativo para Manutencao
            var solicitacao = await _uow.Solicitacao.Get(s => s.Id == manutencao.SolicitacaoId);
            if (solicitacao != null)
            {
                var ativo = await _uow.Ativo.Get(a => a.Id == solicitacao.AtivoId);
                if (ativo != null && ativo.StatusAtivo != Models.Enums.statusAtivo.Manutencao)
                {
                    ativo.StatusAtivo = Models.Enums.statusAtivo.Manutencao;
                    _uow.Ativo.Update(ativo);
                    await _uow.CommitAsync();
                }
            }

            var manutencaoCompleta = await ObterQueryComIncludes()
                .FirstOrDefaultAsync(m => m.Id == id);

            return manutencaoCompleta!.ParaManutencaoDto()!;
        }

        public async Task<ManutencaoDTO> ConcluirManutencao(int id, string? solucaoAplicada = null, decimal? custoReal = null)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == id);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {id} não encontrada.");

            if (manutencao.StatusManutencao != StatusManutencao.EmAndamento)
                throw new InvalidOperationException("Apenas manutenções em andamento podem ser concluídas.");

            manutencao.StatusManutencao = StatusManutencao.Concluida;
            manutencao.DataFechamento = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(solucaoAplicada))
                manutencao.SolucaoAplicada = solucaoAplicada;

            if (custoReal.HasValue)
                manutencao.CustoReal = custoReal.Value;

            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            // Atualizar status do ativo para Disponivel
            var solicitacao = await _uow.Solicitacao.Get(s => s.Id == manutencao.SolicitacaoId);
            if (solicitacao != null)
            {
                var ativo = await _uow.Ativo.Get(a => a.Id == solicitacao.AtivoId);
                if (ativo != null)
                {
                    // Verificar se há outras alocações ativas
                    var temAlocacaoUsuario = await _uow.AtivoUsuario.Any(au => 
                        au.AtivoId == ativo.Id && au.DataFim == null);
                    var temAlocacaoDepartamento = await _uow.AtivoDepartamento.Any(ad => 
                        ad.AtivoId == ativo.Id && ad.DataFim == null);

                    if (temAlocacaoUsuario || temAlocacaoDepartamento)
                        ativo.StatusAtivo = Models.Enums.statusAtivo.EmUso;
                    else
                        ativo.StatusAtivo = Models.Enums.statusAtivo.Disponivel;

                    _uow.Ativo.Update(ativo);
                    await _uow.CommitAsync();
                }
            }

            var manutencaoCompleta = await ObterQueryComIncludes()
                .FirstOrDefaultAsync(m => m.Id == id);

            return manutencaoCompleta!.ParaManutencaoDto()!;
        }

        public async Task<ManutencaoDTO> CancelarManutencao(int id, string? motivo = null)
        {
            var manutencao = await _uow.Manutencao.Get(m => m.Id == id);
            if (manutencao == null)
                throw new KeyNotFoundException($"Manutenção com id {id} não encontrada.");

            if (manutencao.StatusManutencao == StatusManutencao.Concluida)
                throw new InvalidOperationException("Não é possível cancelar uma manutenção já concluída.");

            manutencao.StatusManutencao = StatusManutencao.Cancelada;
            manutencao.DataFechamento = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(motivo))
                manutencao.Observacoes = $"{manutencao.Observacoes}\n[Motivo do Cancelamento]: {motivo}".Trim();

            _uow.Manutencao.Update(manutencao);
            await _uow.CommitAsync();

            // Atualizar status do ativo
            var solicitacao = await _uow.Solicitacao.Get(s => s.Id == manutencao.SolicitacaoId);
            if (solicitacao != null)
            {
                var ativo = await _uow.Ativo.Get(a => a.Id == solicitacao.AtivoId);
                if (ativo != null)
                {
                    // Verificar se há outras alocações ativas
                    var temAlocacaoUsuario = await _uow.AtivoUsuario.Any(au => 
                        au.AtivoId == ativo.Id && au.DataFim == null);
                    var temAlocacaoDepartamento = await _uow.AtivoDepartamento.Any(ad => 
                        ad.AtivoId == ativo.Id && ad.DataFim == null);

                    if (temAlocacaoUsuario || temAlocacaoDepartamento)
                        ativo.StatusAtivo = Models.Enums.statusAtivo.EmUso;
                    else
                        ativo.StatusAtivo = Models.Enums.statusAtivo.Disponivel;

                    _uow.Ativo.Update(ativo);
                    await _uow.CommitAsync();
                }
            }

            var manutencaoCompleta = await ObterQueryComIncludes()
                .FirstOrDefaultAsync(m => m.Id == id);

            return manutencaoCompleta!.ParaManutencaoDto()!;
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarPorAtivo(int ativoId)
        {
            var manutencoes = await ObterQueryComIncludes()
                .Where(m => m.Solicitacao != null && m.Solicitacao.AtivoId == ativoId)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarPorUsuarioResponsavel(int usuarioId)
        {
            var manutencoes = await ObterQueryComIncludes()
                .Where(m => m.UsuarioResponsavelId == usuarioId)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarAtrasadas()
        {
            var hoje = DateTime.UtcNow;
            var manutencoes = await ObterQueryComIncludes()
                .Where(m => 
                    m.DataPrazo.HasValue && 
                    m.DataPrazo.Value < hoje &&
                    m.StatusManutencao != StatusManutencao.Concluida &&
                    m.StatusManutencao != StatusManutencao.Cancelada)
                .OrderBy(m => m.DataPrazo)
                .ToListAsync();

            return manutencoes.ParaListaManutencaoDto();
        }

        public async Task<IEnumerable<ManutencaoDTO>> BuscarPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var manutencoes = await ObterQueryComIncludes()
                .Where(m => m.DataCriacao >= dataInicio && m.DataCriacao <= dataFim)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            return manutencoes.ParaListaManutencaoDto();
        }
    }
}

