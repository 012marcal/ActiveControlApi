using ActiveControlApi.DTO.Usuario;
using ModelUsuario = ActiveControlApi.Models.Usuario;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class UsuarioDtoMapping
    {
        public static UsuarioDTO? ParaUsuarioDto(this ModelUsuario entity)
        {
            if (entity == null) return null;
            return new UsuarioDTO
            {
                Id = entity.Id,
                NomeCompleto = entity.NomeCompleto,
                Cpf = entity.Cpf,
                Email = entity.Email,
                Senha = string.Empty,
                Role = entity.Role
            };
        }

        public static IEnumerable<UsuarioDTO> ParaListaUsuarioDto(this IEnumerable<ModelUsuario> entities)
        {
            if (entities == null || !entities.Any()) return new List<UsuarioDTO>();
            return entities.Select(e => e.ParaUsuarioDto()!).ToList();
        }
    }
}


