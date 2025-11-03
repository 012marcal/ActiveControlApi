using ActiveControlApi.DTO.ModeloAtivo;
using ActiveControlApi.Services.ModeloAtivo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace ActiveControlApi.Controllers.Modelos
{
    [ApiController]
    public class ModeloAtivoController : ControllerBase
    {
        private readonly IModeloAtivoService _service;

        public ModeloAtivoController(IModeloAtivoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/ModeloAtivo")]
        [SwaggerOperation("Adicionar Modelo de Ativo")]
        public async Task<ActionResult<ModeloAtivoDTO>> Adicionar([FromBody] ModeloAtivoDTO dto)
        {
            try
            {
                var criado = await _service.Criar(dto);
                return StatusCode(StatusCodes.Status201Created, criado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/ModeloAtivo")]
        public async Task<ActionResult<IEnumerable<ModeloAtivoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/ModeloAtivo/{id:int}")]
        public async Task<ActionResult<ModeloAtivoDTO>> ObterPorId(int id)
        {
            try
            {
                var item = await _service.PegarPorId(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/ModeloAtivo/{id:int}")]
        public async Task<ActionResult<ModeloAtivoDTO>> Atualizar(int id, [FromBody] ModeloAtivoDTO dto)
        {
            try
            {
                var atualizado = await _service.Atualizar(id, dto);
                return Ok(atualizado);
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
        [Route("v1/ModeloAtivo/{id:int}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var ok = await _service.Remover(id);
                if (!ok)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao remover." });
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}


