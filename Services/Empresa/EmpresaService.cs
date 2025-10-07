using ActiveControlApi.DTO.Empresa;
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

            if (empresas is null)
                throw new Exception("Empresas Nula");

            var EmpresasDTO = empresas.


            return empresas;

        }


    }
}
