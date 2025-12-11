using ActiveControlApi.DTO.AtivoDepartamento;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return list.Any() ? list.ParaListaDto() : Enumerable.Empty<AtivoDepartamentoDTO>();
        }

        public async Task<AtivoDepartamentoDTO> PegarPorId(int id)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");
            return entity.ParaDto();
        }

        public async Task<AtivoDepartamentoDTO> Alocar(AtivoDepartamentoDTO dto)
        {
            bool alocado = await _uow.AtivoUsuario.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null)
                            || await _uow.AtivoDepartamento.Any(x => x.AtivoId == dto.AtivoId && x.DataFim == null);
            if (alocado) throw new InvalidOperationException("Ativo já está alocado (usuário ou departamento).");

            var entity = dto.ParaEntity();
            entity.DataInicio = entity.DataInicio == default ? DateTime.UtcNow : entity.DataInicio;

            if (entity.DataFim.HasValue && entity.DataFim.Value < DateTime.UtcNow)
                throw new InvalidOperationException("Data de término não pode ser no passado.");

            _uow.AtivoDepartamento.Create(entity);
            await _uow.CommitAsync();

            await AtualizarStatusAtivo(dto.AtivoId);

            var departamento = await _uow.Departamento.Get(d => d.Id == dto.DepartamentoId);
            await RegistrarHistoricoMovimentacao(dto.AtivoId, Models.Enums.TipoMovimentacao.AlocacaoDepartamento,
                $"Ativo alocado para departamento {departamento?.Nome ?? dto.DepartamentoId.ToString()}",
                departamentoId: dto.DepartamentoId);

            return entity.ParaDto();
        }

        public async Task<AtivoDepartamentoDTO> Atualizar(int id, AtivoDepartamentoDTO dto)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");

            bool encerrandoAgora = false;

            if (dto.DataFim.HasValue && entity.DataFim == null)
            {
                encerrandoAgora = true;
                entity.DataFim = dto.DataFim.Value;
            }
            else if (dto.DataFim.HasValue)
            {
                entity.DataFim = dto.DataFim.Value;
            }

            if (dto.DataInicio.HasValue)
            {
                entity.DataInicio = dto.DataInicio.Value;
            }

            _uow.AtivoDepartamento.Update(entity);
            await _uow.CommitAsync();

            if (encerrandoAgora) await AtualizarStatusAtivo(entity.AtivoId);

            return entity.ParaDto();
        }

        // ---------------------------------------
        // SOBRECARGA DE ENCERRAR
        // ---------------------------------------
        public async Task<AtivoDepartamentoDTO> Encerrar(int ativoId, DateTime dataFim)
        {
            var entity = await _uow.AtivoDepartamento
                .Get(a => a.AtivoId == ativoId && a.DataFim == null);

            if (entity == null)
                throw new KeyNotFoundException($"Não existe vínculo ativo para o ativo {ativoId}.");

            entity.DataFim = dataFim;

            _uow.AtivoDepartamento.Update(entity);
            await _uow.CommitAsync();

            await AtualizarStatusAtivo(ativoId);

            return entity.ParaDto();
        }


        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.AtivoDepartamento.Get(a => a.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Vínculo AtivoDepartamento {id} não encontrado.");

            _uow.AtivoDepartamento.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }

        public async Task VerificarAlocacoesVencidas()
        {
            var hoje = DateTime.UtcNow;
            var alocacoesVencidas = await _uow.AtivoDepartamento.GetAll(ad =>
                ad.DataFim.HasValue && ad.DataFim.Value.Date <= hoje.Date);

            foreach (var alocacao in alocacoesVencidas)
            {
                await AtualizarStatusAtivo(alocacao.AtivoId);
            }

            await _uow.CommitAsync();
        }

        private async Task AtualizarStatusAtivo(int ativoId)
        {
            var ativo = await _uow.Ativo.GetQueryble().FirstOrDefaultAsync(a => a.Id == ativoId);
            if (ativo == null) return;

            if (ativo.StatusAtivo == Models.Enums.statusAtivo.Manutencao) return;

            bool temUsuario = await _uow.AtivoUsuario.Any(a => a.AtivoId == ativoId && a.DataFim == null);
            bool temDepartamento = await _uow.AtivoDepartamento.Any(a => a.AtivoId == ativoId && a.DataFim == null);

            var novoStatus = (temUsuario || temDepartamento)
                ? Models.Enums.statusAtivo.EmUso
                : Models.Enums.statusAtivo.Disponivel;

            if (ativo.StatusAtivo != novoStatus)
            {
                var statusAnterior = ativo.StatusAtivo;
                ativo.StatusAtivo = novoStatus;
                _uow.Ativo.Update(ativo);
                await _uow.CommitAsync();

                await RegistrarHistoricoMovimentacao(ativoId, Models.Enums.TipoMovimentacao.AlteracaoStatus,
                    $"Status alterado de {statusAnterior} para {novoStatus}");
            }
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
                    UsuarioResponsavelId = usuarioResponsavelId ?? 1,
                    DataMovimentacao = DateTime.UtcNow
                };
                _uow.HistoricoMovimentacao.Create(historico);
            }
            catch
            {
                // Não falhar a operação principal se o histórico falhar
            }
        }
    }
}
