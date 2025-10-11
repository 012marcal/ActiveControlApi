using ActiveControlApi.DTO.Empresas;
using ActiveControlApi.Models;

namespace ActiveControlApi.DTO.MappingExtensions
{
    public static class EmpresaDtoMapping
    {
        

        public static EmpresaDTO? ParaEmpresaDto(this Empresa empresaRegistro )
        {
            if (empresaRegistro == null)
                return null;
            
            return new EmpresaDTO
            {
                RazaoSocial = empresaRegistro.RazaoSocial,
                NomeFantasia = empresaRegistro.NomeFantasia,
                Cnpj = empresaRegistro.Cnpj,
                Email = empresaRegistro.Email,
                TelContato = empresaRegistro.TelContato,
                EnderecoEmpresa = empresaRegistro.EnderecoEmpresa,
                CidadeEmpresa = empresaRegistro.CidadeEmpresa,
                UfEmpresa = empresaRegistro.UfEmpresa

            };

        }


        public static Empresa? ParaEmpresa(this EmpresaDTO empresaDtoRegistro)
        {
            if (empresaDtoRegistro == null)
                return null;

            return new Empresa
            {
                RazaoSocial = empresaDtoRegistro.RazaoSocial,
                NomeFantasia = empresaDtoRegistro.NomeFantasia,
                Cnpj = empresaDtoRegistro.Cnpj,
                Email = empresaDtoRegistro.Email,
                TelContato = empresaDtoRegistro.TelContato,
                EnderecoEmpresa = empresaDtoRegistro.EnderecoEmpresa,
                CidadeEmpresa = empresaDtoRegistro.CidadeEmpresa,
                UfEmpresa = empresaDtoRegistro.UfEmpresa

            };
        }

        public static IEnumerable<EmpresaDTO> ParaListaEmpresaDto(this IEnumerable<Empresa> empresaListRegistro)
        {
            if (empresaListRegistro == null || !empresaListRegistro.Any())
            {
                return new List<EmpresaDTO>();
            }

            return empresaListRegistro.Select(e => new EmpresaDTO
            {
                RazaoSocial = e.RazaoSocial,
                NomeFantasia = e.NomeFantasia,
                Cnpj = e.Cnpj,
                Email = e.Email,
                TelContato = e.TelContato,
                EnderecoEmpresa = e.EnderecoEmpresa,
                CidadeEmpresa = e.CidadeEmpresa,
                UfEmpresa = e.UfEmpresa
            }).ToList();
        }
    }
}
