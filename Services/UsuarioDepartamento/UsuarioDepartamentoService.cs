using ActiveControlApi.DTO.UsuarioDepartamento;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Services.UsuarioDepartamento
{
    public class UsuarioDepartamentoService : IUsuarioDepartamentoService
    {
        private readonly IUnitOfWork _uow;

        public UsuarioDepartamentoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        private IQueryable<Models.UsuarioDepartamento> GetQueryableWithIncludes()
        {
            return _uow.UsuarioDepartamento.GetQueryble()
                .Include(ud => ud.Usuario)
                .Include(ud => ud.Departamento)
                    .ThenInclude(d => d.Empresa);
        }

        public async Task<IEnumerable<UsuarioDepartamentoDTO>> PegarTodos()
        {
            var list = await GetQueryableWithIncludes().ToListAsync();
            if (!list.Any()) return Enumerable.Empty<UsuarioDepartamentoDTO>();
            return list.ParaListaDto();
        }

        public async Task<UsuarioDepartamentoDTO> PegarPorId(int id)
        {
            var entity = await GetQueryableWithIncludes()
                .FirstOrDefaultAsync(ud => ud.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Atribuição UsuarioDepartamento {id} não encontrada.");
            return entity.ParaDto()!;
        }

        public async Task<IEnumerable<UsuarioDepartamentoDTO>> PegarPorUsuario(int usuarioId)
        {
            var list = await GetQueryableWithIncludes()
                .Where(ud => ud.UsuarioId == usuarioId)
                .ToListAsync();
            return list.ParaListaDto();
        }

        public async Task<IEnumerable<UsuarioDepartamentoDTO>> PegarPorDepartamento(int departamentoId)
        {
            var list = await GetQueryableWithIncludes()
                .Where(ud => ud.DepartamentoId == departamentoId)
                .ToListAsync();
            return list.ParaListaDto();
        }

        public async Task<IEnumerable<UsuarioDepartamentoDTO>> PegarPorEmpresa(int empresaId)
        {
            var list = await GetQueryableWithIncludes()
                .Where(ud => ud.Departamento.EmpresaId == empresaId)
                .ToListAsync();
            return list.ParaListaDto();
        }

        private async Task RegistrarHistoricoMovimentacao(int usuarioId, int departamentoId, string descricao)
        {
            try
            {
                var historico = new Models.HistoricoMovimentacao
                {
                    UsuarioId = usuarioId,
                    DepartamentoId = departamentoId,
                    TipoMovimentacao = Models.Enums.TipoMovimentacao.AtribuicaoDepartamento,
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

        public async Task<UsuarioDepartamentoDTO> Atribuir(CriarUsuarioDepartamentoDTO dto)
        {
            // Verificar se usuário existe
            var usuario = await _uow.Usuario.Get(u => u.Id == dto.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuário {dto.UsuarioId} não encontrado.");

            // Verificar se departamento existe
            var departamento = await _uow.Departamento.Get(d => d.Id == dto.DepartamentoId);
            if (departamento == null)
                throw new KeyNotFoundException($"Departamento {dto.DepartamentoId} não encontrado.");

            // Verificar se já existe atribuição ativa (sem DataFim) para este usuário e departamento
            var existeAtiva = await _uow.UsuarioDepartamento.Any(ud => 
                ud.UsuarioId == dto.UsuarioId && 
                ud.DepartamentoId == dto.DepartamentoId && 
                ud.DataFim == null);
            
            if (existeAtiva)
                throw new InvalidOperationException("Usuário já possui atribuição ativa para este departamento.");

            var entity = dto.ParaEntity();
            entity.DataInicio = entity.DataInicio == default ? DateTime.UtcNow : entity.DataInicio;
            
            // Se DataFim foi definida e já passou, não permitir atribuição
            if (entity.DataFim.HasValue && entity.DataFim.Value < DateTime.UtcNow)
                throw new InvalidOperationException("Data de término não pode ser no passado.");

            var created = _uow.UsuarioDepartamento.Create(entity);

            // Registrar histórico
            await RegistrarHistoricoMovimentacao(
                dto.UsuarioId,
                dto.DepartamentoId,
                $"Usuário {usuario.NomeCompleto} atribuído ao departamento {departamento.Nome}"
            );

            await _uow.CommitAsync();
            return created.ParaDto()!;
        }

        public async Task<UsuarioDepartamentoDTO> Atualizar(int id, AtualizarUsuarioDepartamentoDTO dto)
        {
            var entity = await _uow.UsuarioDepartamento.Get(ud => ud.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Atribuição UsuarioDepartamento {id} não encontrada.");

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
                    var departamento = await _uow.Departamento.Get(d => d.Id == entity.DepartamentoId);
                    await RegistrarHistoricoMovimentacao(
                        entity.UsuarioId,
                        entity.DepartamentoId,
                        $"Atribuição de departamento encerrada - DataFim definida: {dto.DataFim.Value:dd/MM/yyyy}"
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

            _uow.UsuarioDepartamento.Update(entity);
            await _uow.CommitAsync();
            return entity.ParaDto()!;
        }

        public async Task<UsuarioDepartamentoDTO> Encerrar(int id)
        {
            var entity = await _uow.UsuarioDepartamento.Get(ud => ud.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Atribuição UsuarioDepartamento {id} não encontrada.");
            if (entity.DataFim != null) throw new InvalidOperationException("Atribuição já encerrada.");
            
            entity.DataFim = DateTime.UtcNow;
            _uow.UsuarioDepartamento.Update(entity);

            // Registrar histórico
            var usuario = await _uow.Usuario.Get(u => u.Id == entity.UsuarioId);
            var departamento = await _uow.Departamento.Get(d => d.Id == entity.DepartamentoId);
            await RegistrarHistoricoMovimentacao(
                entity.UsuarioId,
                entity.DepartamentoId,
                $"Atribuição de departamento {departamento?.Nome} encerrada para usuário {usuario?.NomeCompleto}"
            );

            await _uow.CommitAsync();
            return entity.ParaDto()!;
        }

        public async Task VerificarAtribuicoesVencidas()
        {
            var hoje = DateTime.UtcNow;
            var atribuicoesVencidas = await _uow.UsuarioDepartamento.GetAll(ud => 
                ud.DataFim.HasValue && 
                ud.DataFim.Value <= hoje && 
                ud.DataFim.Value.Date == hoje.Date);

            // As atribuições já estão com DataFim, apenas registrar se necessário
            await _uow.CommitAsync();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.UsuarioDepartamento.Get(ud => ud.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Atribuição UsuarioDepartamento {id} não encontrada.");
            _uow.UsuarioDepartamento.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}

