using ActiveControlApi.DTO.CategoriaAtivo;
using ActiveControlApi.Services.CategoriaAtivo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace ActiveControlApi.Controllers.Categorias
{
    [ApiController]
    public class CategoriaAtivoController : ControllerBase
    {
        private readonly ICategoriaAtivoService _service;

        public CategoriaAtivoController(ICategoriaAtivoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/CategoriaAtivo")]
        [SwaggerOperation("Adicionar Categoria de Ativo")]
        public async Task<ActionResult<CategoriaAtivoDTO>> Adicionar([FromBody] CategoriaAtivoDTO dto)
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
        [Route("v1/CategoriaAtivo")]
        public async Task<ActionResult<IEnumerable<CategoriaAtivoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodas();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/CategoriaAtivo/{id:int}")]
        public async Task<ActionResult<CategoriaAtivoDTO>> ObterPorId(int id)
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
        [Route("v1/CategoriaAtivo/{id:int}")]
        public async Task<ActionResult<CategoriaAtivoDTO>> Atualizar(int id, [FromBody] CategoriaAtivoDTO dto)
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
        [Route("v1/CategoriaAtivo/{id:int}")]
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


