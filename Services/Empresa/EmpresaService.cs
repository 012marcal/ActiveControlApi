using ActiveControlApi.DTO.Empresas;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ActiveControlApi.Services.Empresa
{
    public class EmpresaService : IEmpresaService

    {
        private readonly IUnitOfWork _uow;

        public EmpresaService(IUnitOfWork uow)
        {
            _uow = uow;

            
        }

        public async Task<IEnumerable<EmpresaDTO>> PegarTodas()
        {

            var empresas = await _uow.Empresa.GetAll();

            if(!empresas.Any())
                return Enumerable.Empty<EmpresaDTO>();

            var empresasDto = empresas.ParaListaEmpresaDto();
            return empresasDto;
        }

        public async Task<EmpresaDTO> PegarPorId(int id)
        {
            var empresa = await _uow.Empresa.Get(c => c.Id == id);


            if (empresa == null)
                throw new DirectoryNotFoundException($"Empresa com o {id} não Encontrada");

            var empresaDto = empresa.ParaEmpresaDto();
            return empresaDto;
        }

        public async Task<EmpresaDTO> CriarEmpresa(EmpresaDTO empresaDto)
        {
            var empresa = empresaDto.ParaEmpresa();


            var (valido, jaExiste) = await ValidarEChecarCnpjAsync(empresa.Cnpj);

            if (!valido)
                throw new Exception("CNPJ inválido.");

            if (jaExiste)
                throw new Exception("CNPJ já cadastrado.");


            var empresaCriada =  _uow.Empresa.Create(empresa);
            
            await _uow.CommitAsync();

            var empresaCriadaDto = empresaCriada.ParaEmpresaDto(); 
            
            return empresaCriadaDto;


        }

        //valida Cnpj

        public bool CnpjValido(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            var digits = Regex.Replace(cnpj, @"\D", "");
            if (digits.Length != 14)
                return false;

            if (Enumerable.Range(0, 10).Any(d => new string(char.Parse(d.ToString()), 14) == digits))
                return false;

            var numbers = digits.Select(c => c - '0').ToArray();
            int firstVerifier = CalcularDigito(numbers, new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });
            int secondVerifier = CalcularDigito(numbers, new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });

            return firstVerifier == numbers[12] && secondVerifier == numbers[13];
        }

        private int CalcularDigito(int[] numbers, int[] pesos)
        {
            int soma = 0;
            for (int i = 0; i < pesos.Length; i++)
                soma += numbers[i] * pesos[i];

            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        // 2. Verificar se CNPJ já existe no banco
        public async Task<bool> CnpjJaExisteAsync(string cnpj)
        {
            var digits = Regex.Replace(cnpj, @"\D", "");

            return await _uow.Empresa
                .Any(e => Regex.Replace(e.Cnpj, @"\D", "") == digits);
        }

        // 3. Método que combina validação + existência
        public async Task<(bool valido, bool jaExiste)> ValidarEChecarCnpjAsync(string cnpj)
        {
            bool valido = CnpjValido(cnpj);
            if (!valido)
                return (false, false);

            bool existe = await CnpjJaExisteAsync(cnpj);
            return (true, existe);
        }
    }
}
