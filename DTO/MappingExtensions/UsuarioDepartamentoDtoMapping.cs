using ActiveControlApi.DTO.UsuarioDepartamento;
using Model = ActiveControlApi.Models.UsuarioDepartamento;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class UsuarioDepartamentoDtoMapping
    {
        public static UsuarioDepartamentoDTO? ParaDto(this Model entity)
        {
            if (entity == null) return null;
            return new UsuarioDepartamentoDTO
            {
                Id = entity.Id,
                UsuarioId = entity.UsuarioId,
                UsuarioNome = entity.Usuario?.NomeCompleto,
                DepartamentoId = entity.DepartamentoId,
                DepartamentoNome = entity.Departamento?.Nome,
                EmpresaId = entity.Departamento?.EmpresaId,
                EmpresaNome = entity.Departamento?.Empresa?.RazaoSocial,
                DataInicio = entity.DataInicio,
                DataFim = entity.DataFim
            };
        }

        public static Model? ParaEntity(this CriarUsuarioDepartamentoDTO dto)
        {
            if (dto == null) return null;
            return new Model
            {
                UsuarioId = dto.UsuarioId,
                DepartamentoId = dto.DepartamentoId,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim
            };
        }

        public static IEnumerable<UsuarioDepartamentoDTO> ParaListaDto(this IEnumerable<Model> entities)
        {
            if (entities == null || !entities.Any()) return new List<UsuarioDepartamentoDTO>();
            return entities.Select(e => e.ParaDto()!).ToList();
        }
    }
}





