using ActiveControlApi.DTO.Usuario;
using ActiveControlApi.Services.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Usuarios
{
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/Usuario")]
        [SwaggerOperation(Summary = "Criar um novo Usuário no sistema")]
        [SwaggerResponse(StatusCodes.Status201Created, "Usuário criado com sucesso", typeof(UsuarioDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao criar usuário")]
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
        [SwaggerOperation(Summary = "Listar todos os Usuários cadastrados no sistema")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de usuários obtida com sucesso", typeof(IEnumerable<UsuarioDTO>))]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Usuario/{id:int}")]
        [SwaggerOperation(Summary = "Obter um Usuário específico pelo seu identificador")]
        [SwaggerResponse(StatusCodes.Status200OK, "Usuário encontrado", typeof(UsuarioDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Usuário não encontrado")]
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
        [SwaggerOperation(Summary = "Atualizar informações de um Usuário existente")]
        [SwaggerResponse(StatusCodes.Status200OK, "Usuário atualizado com sucesso", typeof(UsuarioDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Usuário não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao atualizar usuário")]
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
        [SwaggerOperation(Summary = "Remover um Usuário do sistema")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Usuário removido com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Usuário não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao remover usuário")]
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


