using ActiveControlApi.DTO.Empresas;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;

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

        public async Task<EmpresaDTO> CriarEmpresa(EmpresaDTO empresa)
        {

            throw new NotImplementedException();
           // _uow.Empresa.Create()

        }


    }
}
