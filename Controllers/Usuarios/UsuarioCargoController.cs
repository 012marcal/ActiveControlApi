using ActiveControlApi.DTO.UsuarioCargo;
using ActiveControlApi.Services.UsuarioCargo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Usuarios
{
    [ApiController]
    [Authorize]
    public class UsuarioCargoController : ControllerBase
    {
        private readonly IUsuarioCargoService _service;

        public UsuarioCargoController(IUsuarioCargoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/UsuarioCargo")]
        [SwaggerOperation(Summary = "Atribuir Cargo a Usuário")]
        public async Task<ActionResult<UsuarioCargoDTO>> Atribuir([FromBody] CriarUsuarioCargoDTO dto)
        {
            try
            {
                var criado = await _service.Atribuir(dto);
                return StatusCode(StatusCodes.Status201Created, criado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/UsuarioCargo")]
        [SwaggerOperation(Summary = "Listar Todas as Atribuições de Cargo")]
        public async Task<ActionResult<IEnumerable<UsuarioCargoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/UsuarioCargo/{id:int}")]
        [SwaggerOperation(Summary = "Obter Atribuição de Cargo por Id")]
        public async Task<ActionResult<UsuarioCargoDTO>> ObterPorId(int id)
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

        [HttpGet]
        [Route("v1/UsuarioCargo/usuario/{usuarioId:int}")]
        [SwaggerOperation(Summary = "Obter Cargos de um Usuário")]
        public async Task<ActionResult<IEnumerable<UsuarioCargoDTO>>> ObterPorUsuario(int usuarioId)
        {
            var itens = await _service.PegarPorUsuario(usuarioId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/UsuarioCargo/cargo/{cargoId:int}")]
        [SwaggerOperation(Summary = "Obter Usuários de um Cargo")]
        public async Task<ActionResult<IEnumerable<UsuarioCargoDTO>>> ObterPorCargo(int cargoId)
        {
            var itens = await _service.PegarPorCargo(cargoId);
            return Ok(itens);
        }

        [HttpPut]
        [Route("v1/UsuarioCargo/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar Atribuição de Cargo")]
        public async Task<ActionResult<UsuarioCargoDTO>> Atualizar(int id, [FromBody] AtualizarUsuarioCargoDTO dto)
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

        [HttpPut]
        [Route("v1/UsuarioCargo/{id:int}/encerrar")]
        [SwaggerOperation(Summary = "Encerrar Atribuição de Cargo")]
        public async Task<ActionResult<UsuarioCargoDTO>> Encerrar(int id)
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
        [Route("v1/UsuarioCargo/{id:int}")]
        [SwaggerOperation(Summary = "Remover Atribuição de Cargo")]
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


