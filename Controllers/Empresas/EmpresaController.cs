using ActiveControlApi.DTO.Empresas;
using ActiveControlApi.Services.Empresa;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace ActiveControlApi.Controllers.Empresas
{
    //[Route("v1/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {

        private readonly IEmpresaService _empresaService;

        public EmpresaController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;

        }

        [HttpPost]
        [Route("v1/Empresa")]
        [SwaggerOperation("Adição de um(a) Empresa, Adição de um(a) Empresa e retornando o Id do objeto adicionado, logo não é necessário informar o id")]
        [SwaggerResponse(200, "O Id do objeto adicionado: ", typeof(int))]
        public async Task<ActionResult<EmpresaDTO>> Adicionar(EmpresaDTO empresaDTO)
        {
            try
            {
                var empresaCriada = await _empresaService.CriarEmpresa(empresaDTO);
                return StatusCode(StatusCodes.Status201Created, empresaCriada);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message );
            }


        }
    }
}
