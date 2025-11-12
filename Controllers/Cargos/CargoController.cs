using ActiveControlApi.DTO.Cargo;
using ActiveControlApi.Services.Cargo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Cargos
{
    [ApiController]
    [Authorize]
    public class CargoController : ControllerBase
    {
        private readonly ICargoService _service;

        public CargoController(ICargoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/Cargo")]
        [SwaggerOperation(Summary = "Adicionar Cargo")]
        public async Task<ActionResult<CargoDTO>> Adicionar([FromBody] CriarCargoDTO dto)
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
        [Route("v1/Cargo")]
        [SwaggerOperation(Summary = "Listar Cargos")]
        public async Task<ActionResult<IEnumerable<CargoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Cargo/{id:int}")]
        [SwaggerOperation(Summary = "Obter Cargo por Id")]
        public async Task<ActionResult<CargoDTO>> ObterPorId(int id)
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
        [Route("v1/Cargo/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar Cargo")]
        public async Task<ActionResult<CargoDTO>> Atualizar(int id, [FromBody] AtualizarCargoDTO dto)
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
        [Route("v1/Cargo/{id:int}")]
        [SwaggerOperation(Summary = "Remover Cargo")]
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


