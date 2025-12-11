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

        // ----------------------------------------------------------------------
        // PEGAR TODOS
        // ----------------------------------------------------------------------
        public async Task<IEnumerable<AtivoUsuarioDTO>> PegarTodos()
        {
            var list = await _uow.AtivoUsuario.GetAll();
            if (!list.Any()) return Enumerable.Empty<AtivoUsuarioDTO>();
            return list.ParaListaDto();
        }

        // ----------------------------------------------------------------------
        // PEGAR POR ID
        // ----------------------------------------------------------------------
        public async Task<AtivoUsuarioDTO> PegarPorId(int id)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");

            return entity.ParaDto();
        }

        // ----------------------------------------------------------------------
        // ALOCAR
        // ----------------------------------------------------------------------
        public async Task<AtivoUsuarioDTO> Alocar(AtivoUsuarioDTO dto)
        {
            bool abertoUsuario = await _uow.AtivoUsuario.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            bool abertoDepartamento = await _uow.AtivoDepartamento.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);

            if (abertoUsuario || abertoDepartamento)
                throw new InvalidOperationException("Ativo já possui alocação ativa.");

            var entity = dto.ParaEntity();
            entity.DataInicio = DateTime.UtcNow;

            _uow.AtivoUsuario.Create(entity);
            await _uow.CommitAsync();

            // Atualiza o status
            await AtualizarStatusAtivo(dto.AtivoId);

            return entity.ParaDto();
        }

        // ----------------------------------------------------------------------
        // ATUALIZAR
        // ----------------------------------------------------------------------
        public async Task<AtivoUsuarioDTO> Atualizar(int id, AtivoUsuarioDTO dto)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");

            bool estavaEncerrado = entity.DataFim != null;
            bool encerrandoAgora = false;

            if (dto.DataFim.HasValue)
            {
                if (!estavaEncerrado)
                {
                    encerrandoAgora = true;
                    entity.DataFim = dto.DataFim.Value;
                }
                else
                {
                    entity.DataFim = dto.DataFim.Value;
                }
            }

            if (dto.DataInicio.HasValue)
                entity.DataInicio = dto.DataInicio.Value;

            _uow.AtivoUsuario.Update(entity);

            await _uow.CommitAsync();

            if (encerrandoAgora)
                await AtualizarStatusAtivo(entity.AtivoId);

            return entity.ParaDto();
        }

        // ----------------------------------------------------------------------
        // ENCERRAR POR ATIVO
        // ----------------------------------------------------------------------
        public async Task<AtivoUsuarioDTO> Encerrar(int ativoId, DateTime dataFim)
        {
            var entity = await _uow.AtivoUsuario
                .Get(a => a.AtivoId == ativoId && a.DataFim == null);

            if (entity == null)
                throw new KeyNotFoundException($"Não existe vínculo ativo para o ativo {ativoId}.");

            entity.DataFim = dataFim;

            _uow.AtivoUsuario.Update(entity);
            await _uow.CommitAsync();

            await AtualizarStatusAtivo(ativoId);

            return entity.ParaDto();
        }

        // ----------------------------------------------------------------------
        // REMOVER
        // ----------------------------------------------------------------------
        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.AtivoUsuario.Get(a => a.Id == id);
            if (entity == null)
                throw new KeyNotFoundException($"Vínculo AtivoUsuario {id} não encontrado.");

            _uow.AtivoUsuario.Delete(entity);

            var rows = await _uow.CommitAsync();
            return rows > 0;
        }

        // ----------------------------------------------------------------------
        // ATUALIZAR STATUS DO ATIVO
        // ----------------------------------------------------------------------
        private async Task AtualizarStatusAtivo(int ativoId)
        {
            var ativo = await _uow.Ativo.GetQueryble()
                .FirstOrDefaultAsync(a => a.Id == ativoId);

            if (ativo == null)
                return;

            bool temUsuario = await _uow.AtivoUsuario.Any(a => a.AtivoId == ativoId && a.DataFim == null);
            bool temDepartamento = await _uow.AtivoDepartamento.Any(a => a.AtivoId == ativoId && a.DataFim == null);

            var novoStatus = (temUsuario || temDepartamento)
                ? Models.Enums.statusAtivo.EmUso
                : Models.Enums.statusAtivo.Disponivel;

            if (ativo.StatusAtivo != novoStatus)
            {
                ativo.StatusAtivo = novoStatus;
                _uow.Ativo.Update(ativo);
                await _uow.CommitAsync(); // 🔥 AGORA SALVA!
            }
        }

        // ----------------------------------------------------------------------
        // VERIFICAÇÃO DE ALOCAÇÕES AUTOMÁTICAS
        // ----------------------------------------------------------------------
        public async Task VerificarAlocacoesVencidas()
        {
            var abertas = await _uow.AtivoUsuario.GetAll(a => a.DataFim == null);

            if (!abertas.Any())
                return;

            var agora = DateTime.UtcNow;

            foreach (var aloc in abertas)
            {
                if ((agora - aloc.DataInicio).TotalDays >= 365)
                {
                    aloc.DataFim = agora;
                    _uow.AtivoUsuario.Update(aloc);
                    await _uow.CommitAsync();

                    await AtualizarStatusAtivo(aloc.AtivoId);
                }
            }
        }
    }
}
