using ActiveControlApi.DTO.AtivoDepartamento;
using ActiveControlApi.Services.AtivoDepartamento;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActiveControlApi.Controllers.Ativos
{
    [ApiController]
    public class AtivoDepartamentoController : ControllerBase
    {
        private readonly IAtivoDepartamentoService _service;

        public AtivoDepartamentoController(IAtivoDepartamentoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/AtivoDepartamento")]
        public async Task<ActionResult<AtivoDepartamentoDTO>> Alocar([FromBody] AtivoDepartamentoDTO dto)
        {
            try
            {
                var criado = await _service.Alocar(dto);
                return StatusCode(StatusCodes.Status201Created, criado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/AtivoDepartamento")]
        public async Task<ActionResult<IEnumerable<AtivoDepartamentoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/AtivoDepartamento/{id:int}")]
        public async Task<ActionResult<AtivoDepartamentoDTO>> ObterPorId(int id)
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
        [Route("v1/AtivoDepartamento/{id:int}/encerrar")]
        public async Task<ActionResult<AtivoDepartamentoDTO>> Encerrar(int id)
        {
            try
            {
                var atualizado = await _service.Encerrar(id);
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
        [Route("v1/AtivoDepartamento/{id:int}")]
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





