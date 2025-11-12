
using ActiveControlApi.DTO.Empresas;
using ActiveControlApi.DTO.Departamento;

namespace ActiveControlApi.Services.Empresa
{
    public interface IEmpresaService
    {

        Task<IEnumerable<EmpresaDTO>> PegarTodas();
        Task<EmpresaDTO> PegarPorId(int id);
        Task<EmpresaDTO> CriarEmpresa(EmpresaDTO empresaDto);
        bool CnpjValido(string cnpj);
        Task<bool> CnpjJaExisteAsync(string cnpj);
        Task<(bool valido, bool jaExiste)> ValidarEChecarCnpjAsync(string cnpj);
        Task<IEnumerable<EmpresaDTO>> BuscarEmpresas(FiltroEmpresaDTO filtroEmpresa);

        Task<EmpresaDTO> AtualizarEmpresa(int id , EmpresaDTO empresaRegistroDTO);

        Task<bool> RemoverEmpresa(int id);
        Task<IEnumerable<DepartamentoDTO>> ObterDepartamentosPorEmpresa(int empresaId);

   


    }
}
