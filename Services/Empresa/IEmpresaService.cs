using ActiveControlApi.DTO.Empresas;

namespace ActiveControlApi.Services.Empresa
{
    public interface IEmpresaService
    {

        Task<IEnumerable<EmpresaDTO>> PegarTodas();

        Task<EmpresaDTO> PegarPorId(int id);

        Task<EmpresaDTO> CriarEmpresa(EmpresaDTO empresaRegistroDTO);

        public bool CnpjValido(string cnpj);

        int CalcularDigito(int[] numbers, int[] pesos);
        
        Task<bool> CnpjJaExisteAsync(string cnpj);

        Task<(bool valido, bool jaExiste)> ValidarEChecarCnpjAsync(string cnpj);

        Task<EmpresaDTO> AtualizarEmpresa(int id , EmpresaDTO empresaRegistroDTO);

        Task<bool> RemoverEmpresa(int id);


    }
}
