using ActiveControlApi.DTO.Ativo;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActiveControlApi.Services.Ativo
{
    public class AtivoService : IAtivoService
    {
        private readonly IUnitOfWork _uow;

        public AtivoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<AtivoDTO>> PegarTodos()
        {
            var ativos = await _uow.Ativo.GetAll();
            if (!ativos.Any())
                return Enumerable.Empty<AtivoDTO>();

            return ativos.ParaListaAtivoDto();
        }

        public async Task<AtivoDTO> PegarPorId(int idAtivo)
        {
            var ativo = await _uow.Ativo.Get(a => a.Id == idAtivo);
            if (ativo == null)
                throw new KeyNotFoundException($"Ativo com id {idAtivo} não encontrado.");

            return ativo.ParaAtivoDto();
        }

        public async Task<AtivoDTO> CriarAtivo(AtivoDTO ativoRegistro)
        {
            var entity = ativoRegistro.ParaAtivo();

            var criado = _uow.Ativo.Create(entity);
            await _uow.CommitAsync();

            return criado.ParaAtivoDto();
        }

        public async Task<AtivoDTO> AtualizarAtivo(int idAtivo, AtivoDTO ativoRegistro)
        {
            var entity = await _uow.Ativo.Get(a => a.Id == idAtivo);
            if (entity == null)
                throw new KeyNotFoundException($"Ativo com id {idAtivo} não encontrado.");

            if (!string.IsNullOrWhiteSpace(ativoRegistro.AtivoNome))
                entity.AtivoNome = ativoRegistro.AtivoNome;
            if (!string.IsNullOrWhiteSpace(ativoRegistro.NumPatrimonio))
                entity.NumPatrimonio = ativoRegistro.NumPatrimonio;
            if (!string.IsNullOrWhiteSpace(ativoRegistro.NumSerie))
                entity.NumSerie = ativoRegistro.NumSerie;

            if (ativoRegistro.ValorAquisicao > 0)
                entity.ValorAquisicao = ativoRegistro.ValorAquisicao;
            if (ativoRegistro.DataAquisicao != default)
                entity.DataAquisicao = ativoRegistro.DataAquisicao;
            if (ativoRegistro.StatusAtivo.HasValue)
                entity.StatusAtivo = (Models.Enums.statusAtivo)ativoRegistro.StatusAtivo.Value;

            if (ativoRegistro.ModeloAtivoId > 0)
                entity.ModeloAtivoId = ativoRegistro.ModeloAtivoId;
            if (ativoRegistro.CategoriaAtivoId > 0)
                entity.CategoriaAtivoId = ativoRegistro.CategoriaAtivoId;

            if (ativoRegistro.VidaUtilEstimadaAnos > 0)
                entity.VidaUtilEstimadaAnos = ativoRegistro.VidaUtilEstimadaAnos;
            if (ativoRegistro.TaxaDepreciacaoAnual > 0)
                entity.TaxaDepreciacaoAnual = ativoRegistro.TaxaDepreciacaoAnual;

            _uow.Ativo.Update(entity);
            await _uow.CommitAsync();

            return entity.ParaAtivoDto();
        }

        public async Task<bool> RemoverAtivo(int idAtivo)
        {
            var entity = await _uow.Ativo.Get(a => a.Id == idAtivo);
            if (entity == null)
                throw new KeyNotFoundException($"Ativo com id {idAtivo} não encontrado.");

            _uow.Ativo.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }
    }
}


