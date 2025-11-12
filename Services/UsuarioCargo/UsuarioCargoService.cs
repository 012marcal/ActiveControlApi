using ActiveControlApi.DTO.UsuarioCargo;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Services.UsuarioCargo
{
    public class UsuarioCargoService : IUsuarioCargoService
    {
        private readonly IUnitOfWork _uow;

        public UsuarioCargoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        private IQueryable<Models.UsuarioCargo> GetQueryableWithIncludes()
        {
            return _uow.UsuarioCargo.GetQueryble()
                .Include(uc => uc.Usuario)
                .Include(uc => uc.Cargo);
        }

        public async Task<IEnumerable<UsuarioCargoDTO>> PegarTodos()
        {
            var list = await GetQueryableWithIncludes().ToListAsync();
            if (!list.Any()) return Enumerable.Empty<UsuarioCargoDTO>();
            return list.ParaListaDto();
        }

        public async Task<UsuarioCargoDTO> PegarPorId(int id)
        {
            var entity = await GetQueryableWithIncludes()
                .FirstOrDefaultAsync(uc => uc.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Atribuição UsuarioCargo {id} não encontrada.");
            return entity.ParaDto()!;
        }

        public async Task<IEnumerable<UsuarioCargoDTO>> PegarPorUsuario(int usuarioId)
        {
            var list = await GetQueryableWithIncludes()
                .Where(uc => uc.UsuarioId == usuarioId)
                .ToListAsync();
            return list.ParaListaDto();
        }

        public async Task<IEnumerable<UsuarioCargoDTO>> PegarPorCargo(int cargoId)
        {
            var list = await GetQueryableWithIncludes()
                .Where(uc => uc.CargoId == cargoId)
                .ToListAsync();
            return list.ParaListaDto();
        }

        private async Task RegistrarHistoricoMovimentacao(int usuarioId, int cargoId, string descricao)
        {
            try
            {
                var historico = new Models.HistoricoMovimentacao
                {
                    UsuarioId = usuarioId,
                    TipoMovimentacao = Models.Enums.TipoMovimentacao.AtribuicaoCargo,
                    Descricao = descricao,
                    DataMovimentacao = DateTime.UtcNow
                };
                _uow.HistoricoMovimentacao.Create(historico);
            }
            catch
            {
                // Não falhar a operação principal se o histórico falhar
            }
        }

        public async Task<UsuarioCargoDTO> Atribuir(CriarUsuarioCargoDTO dto)
        {
            // Verificar se usuário existe
            var usuario = await _uow.Usuario.Get(u => u.Id == dto.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuário {dto.UsuarioId} não encontrado.");

            // Verificar se cargo existe
            var cargo = await _uow.Cargo.Get(c => c.Id == dto.CargoId);
            if (cargo == null)
                throw new KeyNotFoundException($"Cargo {dto.CargoId} não encontrado.");

            // Verificar se já existe atribuição ativa (sem DataFim) para este usuário e cargo
            var existeAtiva = await _uow.UsuarioCargo.Any(uc => 
                uc.UsuarioId == dto.UsuarioId && 
                uc.CargoId == dto.CargoId && 
                uc.DataFim == null);
            
            if (existeAtiva)
                throw new InvalidOperationException("Usuário já possui atribuição ativa para este cargo.");

            var entity = dto.ParaEntity();
            entity.DataInicio = entity.DataInicio == default ? DateTime.UtcNow : entity.DataInicio;
            
            // Se DataFim foi definida e já passou, não permitir atribuição
            if (entity.DataFim.HasValue && entity.DataFim.Value < DateTime.UtcNow)
                throw new InvalidOperationException("Data de término não pode ser no passado.");

            var created = _uow.UsuarioCargo.Create(entity);

            // Registrar histórico
            await RegistrarHistoricoMovimentacao(
                dto.UsuarioId,
                dto.CargoId,
                $"Cargo {cargo.Descricao} atribuído ao usuário {usuario.NomeCompleto}"
            );

            await _uow.CommitAsync();
            return created.ParaDto()!;
        }

        public async Task<UsuarioCargoDTO> Atualizar(int id, AtualizarUsuarioCargoDTO dto)
        {
            var entity = await _uow.UsuarioCargo.Get(uc => uc.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Atribuição UsuarioCargo {id} não encontrada.");

            var estavaEncerrado = entity.DataFim != null;

            // Validar DataFim
            if (dto.DataFim.HasValue)
            {
                if (dto.DataFim.Value < entity.DataInicio)
                    throw new InvalidOperationException("Data de término não pode ser anterior à data de início.");

                // Se está definindo DataFim e antes não tinha, está encerrando o vínculo
                if (!estavaEncerrado)
                {
                    entity.DataFim = dto.DataFim.Value;
                    
                    var usuario = await _uow.Usuario.Get(u => u.Id == entity.UsuarioId);
                    var cargo = await _uow.Cargo.Get(c => c.Id == entity.CargoId);
                    await RegistrarHistoricoMovimentacao(
                        entity.UsuarioId,
                        entity.CargoId,
                        $"Atribuição de cargo encerrada - DataFim definida: {dto.DataFim.Value:dd/MM/yyyy}"
                    );
                }
                else
                {
                    entity.DataFim = dto.DataFim.Value;
                }
            }
            else if (dto.DataFim == null && estavaEncerrado)
            {
                throw new InvalidOperationException("Não é possível reabrir uma atribuição encerrada. Crie uma nova atribuição.");
            }

            if (dto.DataInicio != entity.DataInicio)
            {
                if (dto.DataInicio > (entity.DataFim ?? DateTime.MaxValue))
                    throw new InvalidOperationException("Data de início não pode ser posterior à data de término.");
                entity.DataInicio = dto.DataInicio;
            }

            _uow.UsuarioCargo.Update(entity);
            await _uow.CommitAsync();
            return entity.ParaDto()!;
        }

        public async Task<UsuarioCargoDTO> Encerrar(int id)
        {
            var entity = await _uow.UsuarioCargo.Get(uc => uc.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Atribuição UsuarioCargo {id} não encontrada.");
            if (entity.DataFim != null) throw new InvalidOperationException("Atribuição já encerrada.");
            
            entity.DataFim = DateTime.UtcNow;
            _uow.UsuarioCargo.Update(entity);

            // Registrar histórico
            var usuario = await _uow.Usuario.Get(u => u.Id == entity.UsuarioId);
            var cargo = await _uow.Cargo.Get(c => c.Id == entity.CargoId);
            await RegistrarHistoricoMovimentacao(
                entity.UsuarioId,
                entity.CargoId,
                $"Atribuição de cargo {cargo?.Descricao} encerrada para usuário {usuario?.NomeCompleto}"
            );

            await _uow.CommitAsync();
            return entity.ParaDto()!;
        }

        public async Task VerificarAtribuicoesVencidas()
        {
            var hoje = DateTime.UtcNow;
            var atribuicoesVencidas = await _uow.UsuarioCargo.GetAll(uc => 
                uc.DataFim.HasValue && 
                uc.DataFim.Value <= hoje && 
                uc.DataFim.Value.Date == hoje.Date);

            // As atribuições já estão com DataFim, apenas registrar se necessário
            await _uow.CommitAsync();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.UsuarioCargo.Get(uc => uc.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Atribuição UsuarioCargo {id} não encontrada.");
            _uow.UsuarioCargo.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}

