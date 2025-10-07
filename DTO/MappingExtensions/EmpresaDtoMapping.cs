using ActiveControlApi.DTO.Empresas;
using ActiveControlApi.Models;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class EmpresaDtoMapping
    {
        

        public static EmpresaDTO ParaEmpresaDto(this Empresa empresaRegistro )
        {
            
            
            return new EmpresaDTO
            {
                RazaoSocial = empresaRegistro.RazaoSocial,
                NomeFantasia = empresaRegistro.NomeFantasia,

            };

        }

    }
}
