using ActiveControlApi.DTO.AtivoUsuario;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Services.AtivoUsuario
{
    public class AtivoUsuarioService : IAtivoUsuarioService
    {
        private readonly IUnitOfWork _uow;

        public AtivoUsuarioService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<AtivoUsuarioDTO>> PegarTodos()
        {
            var list = await _uow.AtivoUsuario.GetAll();
            if (!list.Any()) return Enumerable.Empty<AtivoUsuarioDTO>();
            return list.ParaListaDto();
        }

        public async Task<AtivoUsuarioDTO> PegarPorId(int id)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");
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
                    UsuarioResponsavelId = usuarioResponsavelId ?? usuarioId ?? 1, // Fallback para sistema
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

        public async Task<AtivoUsuarioDTO> Alocar(AtivoUsuarioDTO dto)
        {
            // regra: não permitir alocação se já existe alocação aberta do ativo
            bool abertoUsuario = await _uow.AtivoUsuario.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            bool abertoDepartamento = await _uow.AtivoDepartamento.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            if (abertoUsuario || abertoDepartamento)
                throw new InvalidOperationException("Ativo já está alocado (usuário ou departamento).");

            var entity = dto.ParaEntity();
            entity.DataInicio = entity.DataInicio == default ? DateTime.UtcNow : entity.DataInicio;
            
            // Se DataFim foi definida e já passou, não permitir alocação
            if (entity.DataFim.HasValue && entity.DataFim.Value < DateTime.UtcNow)
                throw new InvalidOperationException("Data de término não pode ser no passado.");

            var created = _uow.AtivoUsuario.Create(entity);

            // Atualizar status do ativo
            await AtualizarStatusAtivo(dto.AtivoId);

            // Registrar histórico
            var ativo = await _uow.Ativo.Get(a => a.Id == dto.AtivoId);
            var usuario = await _uow.Usuario.Get(u => u.Id == dto.UsuarioId);
            await RegistrarHistoricoMovimentacao(
                dto.AtivoId,
                Models.Enums.TipoMovimentacao.AlocacaoUsuario,
                $"Ativo alocado para usuário {usuario?.NomeCompleto ?? dto.UsuarioId.ToString()}",
                usuarioId: dto.UsuarioId
            );

            await _uow.CommitAsync();
            return created.ParaDto();
        }

        public async Task<AtivoUsuarioDTO> Atualizar(int id, AtivoUsuarioDTO dto)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");

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
                    
                    var usuario = await _uow.Usuario.Get(u => u.Id == entity.UsuarioId);
                    await RegistrarHistoricoMovimentacao(
                        entity.AtivoId,
                        Models.Enums.TipoMovimentacao.DesalocacaoUsuario,
                        $"Vínculo encerrado - DataFim definida: {dto.DataFim.Value:dd/MM/yyyy}",
                        usuarioId: entity.UsuarioId
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

            _uow.AtivoUsuario.Update(entity);
            
            // Se encerrou o vínculo, atualizar status do ativo
            if (encerrandoAgora)
            {
                await AtualizarStatusAtivo(entity.AtivoId);
            }

            await _uow.CommitAsync();
            return entity.ParaDto();
        }

        public async Task<AtivoUsuarioDTO> Encerrar(int id)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");
            if (entity.DataFim != null) throw new InvalidOperationException("Vínculo já encerrado.");
            
            entity.DataFim = DateTime.UtcNow;
            _uow.AtivoUsuario.Update(entity);

            // Atualizar status do ativo
            await AtualizarStatusAtivo(entity.AtivoId);

            // Registrar histórico
            var usuario = await _uow.Usuario.Get(u => u.Id == entity.UsuarioId);
            await RegistrarHistoricoMovimentacao(
                entity.AtivoId,
                Models.Enums.TipoMovimentacao.DesalocacaoUsuario,
                $"Alocação encerrada para usuário {usuario?.NomeCompleto ?? entity.UsuarioId.ToString()}",
                usuarioId: entity.UsuarioId
            );

            await _uow.CommitAsync();
            return entity.ParaDto();
        }

        public async Task VerificarAlocacoesVencidas()
        {
            var hoje = DateTime.UtcNow;
            var alocacoesVencidas = await _uow.AtivoUsuario.GetAll(au => 
                au.DataFim.HasValue && 
                au.DataFim.Value <= hoje && 
                au.DataFim.Value.Date == hoje.Date); // Apenas as que vencem hoje

            foreach (var alocacao in alocacoesVencidas)
            {
                if (alocacao.DataFim.HasValue && alocacao.DataFim.Value <= hoje)
                {
                    // Já está com DataFim, apenas atualizar status se necessário
                    await AtualizarStatusAtivo(alocacao.AtivoId);
                }
            }

            // Verificar alocações que deveriam ter sido encerradas mas ainda estão abertas
            var alocacoesAbertasComDataFimPassada = await _uow.AtivoUsuario.GetAll(au => 
                au.DataFim == null);

            // Não há como verificar DataFim se ela não existe, então isso fica para o método Atualizar
            await _uow.CommitAsync();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");
            _uow.AtivoUsuario.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}


