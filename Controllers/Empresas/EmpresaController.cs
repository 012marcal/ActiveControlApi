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

        [HttpGet]
        [Route("v1/Empresa")]
        [SwaggerOperation("Obter todas as Empresas cadastradas.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de empresas obtida com sucesso.", typeof(IEnumerable<EmpresaDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nenhuma empresa encontrada.")]
        public async Task<ActionResult<IEnumerable<EmpresaDTO>>> ObterTodas()
        {
            var empresas = await _empresaService.PegarTodas();

            if (!empresas.Any())
                return NotFound(new { message = "Nenhuma empresa encontrada." });

            return Ok(empresas);
        }

        [HttpGet]
        [Route("v1/Empresa/{id:int}")]
        [SwaggerOperation("Obter uma Empresa pelo seu Id.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Empresa encontrada.", typeof(EmpresaDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empresa não encontrada.")]
        public async Task<ActionResult<EmpresaDTO>> ObterPorId(int id)
        {
            try
            {
                var empresa = await _empresaService.PegarPorId(id);
                return Ok(empresa);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/Empresa/{id:int}")]
        [EndpointSummary("Atualizar Empresa")]
        [SwaggerOperation("Atualizar uma Empresa existente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Empresa atualizada com sucesso.", typeof(EmpresaDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empresa não encontrada.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao atualizar a empresa.")]
        public async Task<ActionResult<EmpresaDTO>> Atualizar(int id, [FromBody] EmpresaDTO empresaDTO)
        {
            try
            {
                var empresaAtualizada = await _empresaService.AtualizarEmpresa(id, empresaDTO);
                return Ok(empresaAtualizada);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
        [HttpDelete]
        [Route("v1/Empresa/{id:int}")]
        [SwaggerOperation("Remover uma Empresa existente pelo Id.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Empresa removida com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empresa não encontrada.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro ao remover a empresa.")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var sucesso = await _empresaService.RemoverEmpresa(id);

                if (!sucesso)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Erro ao remover a empresa." });

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Empresa/Buscar")]
        [SwaggerOperation("Buscar Empresas por filtros: Razão Social, CNPJ, Cidade ou Estado.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Empresas encontradas.", typeof(IEnumerable<EmpresaDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nenhuma empresa encontrada com os filtros fornecidos.")]
        public async Task<ActionResult<IEnumerable<EmpresaDTO>>> Buscar([FromQuery] FiltroEmpresaDTO filtro)
        {
            var empresas = await _empresaService.BuscarEmpresas(filtro);

            if (!empresas.Any())
                return NotFound(new { message = "Nenhuma empresa encontrada com os filtros fornecidos." });

            return Ok(empresas);
        }
    }
}
