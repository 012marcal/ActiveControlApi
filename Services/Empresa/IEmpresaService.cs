using ActiveControlApi.DTO.Empresas;

namespace ActiveControlApi.Services.Empresa
{
    public interface IEmpresaService
    {

        Task<IEnumerable<EmpresaDTO>> PegarTodas();

        Task<EmpresaDTO> PegarPorId(int id);

        Task<EmpresaDTO> CriarEmpresa(EmpresaDTO empresaRegistroDTO);

        Task<EmpresaDTO> AtualizarEmpresa(int id , EmpresaDTO empresaRegistroDTO);

        Task<bool> RemoverEmpresa(int id);


    }
}
