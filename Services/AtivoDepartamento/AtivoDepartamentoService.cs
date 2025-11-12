using ActiveControlApi.DTO.AtivoDepartamento;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;

namespace ActiveControlApi.Services.AtivoDepartamento
{
    public class AtivoDepartamentoService : IAtivoDepartamentoService
    {
        private readonly IUnitOfWork _uow;

        public AtivoDepartamentoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<AtivoDepartamentoDTO>> PegarTodos()
        {
            var list = await _uow.AtivoDepartamento.GetAll();
            if (!list.Any()) return Enumerable.Empty<AtivoDepartamentoDTO>();
            return list.ParaListaDto();
        }

        public async Task<AtivoDepartamentoDTO> PegarPorId(int id)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");
            return entity.ParaDto();
        }

        private async Task RegistrarHistoricoMovimentacao(int ativoId, Models.Enums.TipoMovimentacao tipo, string descricao, int? usuarioId = null, int? departamentoId = null, int? solicitacaoId = null, int? usuarioResponsavelId = null)
        {
            try
            {
                var historico = new Models.HistoricoMovimentacao
                {
                    AtivoId = ativoId,
                    TipoMovimentacao = tipo,
                    Descricao = descricao,
                    UsuarioId = usuarioId,
                    DepartamentoId = departamentoId,
                    SolicitacaoId = solicitacaoId,
                    UsuarioResponsavelId = usuarioResponsavelId ?? 1, // Fallback para sistema
                    DataMovimentacao = DateTime.UtcNow
                };
                _uow.HistoricoMovimentacao.Create(historico);
            }
            catch
            {
                // Não falhar a operação principal se o histórico falhar
            }
        }

        private async Task AtualizarStatusAtivo(int ativoId)
        {
            var ativo = await _uow.Ativo.Get(a => a.Id == ativoId);
            if (ativo == null) return;

            // Verificar se está em manutenção
            if (ativo.StatusAtivo == Models.Enums.statusAtivo.Manutencao)
                return; // Não alterar se estiver em manutenção

            // Verificar alocações ativas
            var temAlocacaoUsuario = await _uow.AtivoUsuario.Any(au => 
                au.AtivoId == ativoId && au.DataFim == null);
            var temAlocacaoDepartamento = await _uow.AtivoDepartamento.Any(ad => 
                ad.AtivoId == ativoId && ad.DataFim == null);

            var novoStatus = (temAlocacaoUsuario || temAlocacaoDepartamento) 
                ? Models.Enums.statusAtivo.EmUso 
                : Models.Enums.statusAtivo.Disponivel;

            if (ativo.StatusAtivo != novoStatus)
            {
                var statusAnterior = ativo.StatusAtivo;
                ativo.StatusAtivo = novoStatus;
                _uow.Ativo.Update(ativo);
                await RegistrarHistoricoMovimentacao(
                    ativoId, 
                    Models.Enums.TipoMovimentacao.AlteracaoStatus, 
                    $"Status alterado de {statusAnterior} para {novoStatus}"
                );
            }
        }

        public async Task<AtivoDepartamentoDTO> Alocar(AtivoDepartamentoDTO dto)
        {
            bool abertoUsuario = await _uow.AtivoUsuario.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            bool abertoDepartamento = await _uow.AtivoDepartamento.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            if (abertoUsuario || abertoDepartamento)
                throw new InvalidOperationException("Ativo já está alocado (usuário ou departamento).");

            var entity = dto.ParaEntity();
            entity.DataInicio = entity.DataInicio == default ? DateTime.UtcNow : entity.DataInicio;
            
            // Se DataFim foi definida e já passou, não permitir alocação
            if (entity.DataFim.HasValue && entity.DataFim.Value < DateTime.UtcNow)
                throw new InvalidOperationException("Data de término não pode ser no passado.");

            var created = _uow.AtivoDepartamento.Create(entity);

            // Atualizar status do ativo
            await AtualizarStatusAtivo(dto.AtivoId);

            // Registrar histórico
            var departamento = await _uow.Departamento.Get(d => d.Id == dto.DepartamentoId);
            await RegistrarHistoricoMovimentacao(
                dto.AtivoId,
                Models.Enums.TipoMovimentacao.AlocacaoDepartamento,
                $"Ativo alocado para departamento {departamento?.Nome ?? dto.DepartamentoId.ToString()}",
                departamentoId: dto.DepartamentoId
            );

            await _uow.CommitAsync();
            return created.ParaDto();
        }

        public async Task<AtivoDepartamentoDTO> Atualizar(int id, AtivoDepartamentoDTO dto)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");

            var estavaEncerrado = entity.DataFim != null;
            var encerrandoAgora = false;

            // Validar DataFim
            if (dto.DataFim.HasValue)
            {
                if (dto.DataFim.Value < entity.DataInicio)
                    throw new InvalidOperationException("Data de término não pode ser anterior à data de início.");

                // Se está definindo DataFim e antes não tinha, está encerrando o vínculo
                if (!estavaEncerrado)
                {
                    encerrandoAgora = true;
                    entity.DataFim = dto.DataFim.Value;
                    
                    var departamento = await _uow.Departamento.Get(d => d.Id == entity.DepartamentoId);
                    await RegistrarHistoricoMovimentacao(
                        entity.AtivoId,
                        Models.Enums.TipoMovimentacao.DesalocacaoDepartamento,
                        $"Vínculo encerrado - DataFim definida: {dto.DataFim.Value:dd/MM/yyyy}",
                        departamentoId: entity.DepartamentoId
                    );
                }
                else
                {
                    // Apenas atualizar DataFim se já estava encerrado
                    entity.DataFim = dto.DataFim.Value;
                }
            }
            else if (dto.DataFim == null && estavaEncerrado)
            {
                // Removendo DataFim (reabrindo vínculo) - não permitido por regra de negócio
                throw new InvalidOperationException("Não é possível reabrir um vínculo encerrado. Crie uma nova alocação.");
            }

            if (dto.DataInicio.HasValue && dto.DataInicio.Value != entity.DataInicio)
            {
                if (dto.DataInicio.Value > (entity.DataFim ?? DateTime.MaxValue))
                    throw new InvalidOperationException("Data de início não pode ser posterior à data de término.");
                entity.DataInicio = dto.DataInicio.Value;
            }

            _uow.AtivoDepartamento.Update(entity);
            
            // Se encerrou o vínculo, atualizar status do ativo
            if (encerrandoAgora)
            {
                await AtualizarStatusAtivo(entity.AtivoId);
            }

            await _uow.CommitAsync();
            return entity.ParaDto();
        }

        public async Task<AtivoDepartamentoDTO> Encerrar(int id)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");
            if (entity.DataFim != null) throw new InvalidOperationException("Vínculo já encerrado.");
            
            entity.DataFim = DateTime.UtcNow;
            _uow.AtivoDepartamento.Update(entity);

            // Atualizar status do ativo
            await AtualizarStatusAtivo(entity.AtivoId);

            // Registrar histórico
            var departamento = await _uow.Departamento.Get(d => d.Id == entity.DepartamentoId);
            await RegistrarHistoricoMovimentacao(
                entity.AtivoId,
                Models.Enums.TipoMovimentacao.DesalocacaoDepartamento,
                $"Alocação encerrada para departamento {departamento?.Nome ?? entity.DepartamentoId.ToString()}",
                departamentoId: entity.DepartamentoId
            );

            await _uow.CommitAsync();
            return entity.ParaDto();
        }

        public async Task VerificarAlocacoesVencidas()
        {
            var hoje = DateTime.UtcNow;
            var alocacoesVencidas = await _uow.AtivoDepartamento.GetAll(ad => 
                ad.DataFim.HasValue && 
                ad.DataFim.Value <= hoje && 
                ad.DataFim.Value.Date == hoje.Date);

            foreach (var alocacao in alocacoesVencidas)
            {
                if (alocacao.DataFim.HasValue && alocacao.DataFim.Value <= hoje)
                {
                    await AtualizarStatusAtivo(alocacao.AtivoId);
                }
            }

            await _uow.CommitAsync();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");
            _uow.AtivoDepartamento.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}


