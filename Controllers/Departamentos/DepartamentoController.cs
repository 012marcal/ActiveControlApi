using ActiveControlApi.DTO.Departamento;
using ActiveControlApi.Services.Departamento;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace ActiveControlApi.Controllers.Departamentos
{
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoService _service;

        public DepartamentoController(IDepartamentoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/Departamento")]
        [SwaggerOperation("Adicionar Departamento")]
        public async Task<ActionResult<DepartamentoDTO>> Adicionar([FromBody] DepartamentoDTO dto)
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
        [Route("v1/Departamento")]
        [SwaggerOperation("Listar Departamentos")]
        public async Task<ActionResult<IEnumerable<DepartamentoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Departamento/{id:int}")]
        [SwaggerOperation("Obter Departamento por Id")]
        public async Task<ActionResult<DepartamentoDTO>> ObterPorId(int id)
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
        [Route("v1/Departamento/{id:int}")]
        [SwaggerOperation("Atualizar Departamento")]
        public async Task<ActionResult<DepartamentoDTO>> Atualizar(int id, [FromBody] DepartamentoDTO dto)
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
        [Route("v1/Departamento/{id:int}")]
        [SwaggerOperation("Remover Departamento")]
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
        }
    }
}





