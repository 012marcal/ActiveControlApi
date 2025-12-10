using ActiveControlApi.DTO.Ativo;
using ActiveControlApi.DTO.CategoriaAtivo;
using ActiveControlApi.DTO.ModeloAtivo;
using ModelAtivo = ActiveControlApi.Models.Ativo;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class AtivoCompletoDtoMapping
    {
        public static AtivoCompletoDTO? ParaAtivoCompletoDto(this ModelAtivo ativo)
        {
            if (ativo == null)
                return null;

            // Obtém o responsável atual (DataFim == null)
            var relUsuario = ativo.AtivoUsuario?.FirstOrDefault(x => x.DataFim == null);
            var relDepartamento = ativo.AtivoDepartamento?.FirstOrDefault(x => x.DataFim == null);

            // Define responsável (usuário ou departamento)
            ResponsavelAtivoDTO? responsavel = null;

            if (relUsuario != null)
            {
                responsavel = new ResponsavelAtivoDTO
                {
                    Tipo = "Usuario",
                    Id = relUsuario.UsuarioId,
                    Nome = relUsuario.Usuario?.NomeCompleto
                };
            }
            else if (relDepartamento != null)
            {
                responsavel = new ResponsavelAtivoDTO
                {
                    Tipo = "Departamento",
                    Id = relDepartamento.DepartamentoId,
                    Nome = relDepartamento.Departamento?.Nome
                };
            }

            return new AtivoCompletoDTO
            {
                Id = ativo.Id,
                AtivoNome = ativo.AtivoNome,
                NumPatrimonio = ativo.NumPatrimonio,
                NumSerie = ativo.NumSerie,
                ValorAquisicao = ativo.ValorAquisicao,
                DataAquisicao = ativo.DataAquisicao,
                StatusAtivo = ativo.StatusAtivo,
                VidaUtilEstimadaAnos = ativo.VidaUtilEstimadaAnos,
                TaxaDepreciacaoAnual = ativo.TaxaDepreciacaoAnual,

                Modelo = ativo.ModeloAtivo != null ? new ModeloAtivoDTO
                {
                    Id = ativo.ModeloAtivo.Id,
                    Nome = ativo.ModeloAtivo.Nome,
                    Fabricante = ativo.ModeloAtivo.Fabricante,
                    Especificacoes = ativo.ModeloAtivo.Especificacoes
                } : null,

                Categoria = ativo.CategoriaAtivo != null ? new CategoriaAtivoDTO
                {
                    Id = ativo.CategoriaAtivo.Id,
                    NomeCategoria = ativo.CategoriaAtivo.NomeCategoria
                } : null,

                // novo:
                Responsavel = responsavel
            };
        }

        public static IEnumerable<AtivoCompletoDTO> ParaListaAtivoCompletoDto(this IEnumerable<ModelAtivo> ativos)
        {
            if (ativos == null)
                return Enumerable.Empty<AtivoCompletoDTO>();

            return ativos
                .Select(a => a.ParaAtivoCompletoDto())
                .Where(dto => dto != null)!
                .ToList();
        }
    }
}
