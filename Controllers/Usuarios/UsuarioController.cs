using ActiveControlApi.DTO.Usuario;
using ActiveControlApi.Services.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace ActiveControlApi.Controllers.Usuarios
{
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/Usuario")]
        [SwaggerOperation("Adicionar Usuário")]
        public async Task<ActionResult<UsuarioDTO>> Adicionar([FromBody] UsuarioDTO dto)
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
        [Route("v1/Usuario")]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Usuario/{id:int}")]
        public async Task<ActionResult<UsuarioDTO>> ObterPorId(int id)
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
        [Route("v1/Usuario/{id:int}")]
        public async Task<ActionResult<UsuarioDTO>> Atualizar(int id, [FromBody] UsuarioDTO dto)
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
        [Route("v1/Usuario/{id:int}")]
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


