using ActiveControlApi.DTO.AtivoUsuario;
using ActiveControlApi.Services.AtivoUsuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Ativos
{
    [ApiController]
    [Authorize]
    public class AtivoUsuarioController : ControllerBase
    {
        private readonly IAtivoUsuarioService _service;

        public AtivoUsuarioController(IAtivoUsuarioService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/AtivoUsuario")]
        [SwaggerOperation(Summary = "Alocar um Ativo para um Usuário específico")]
        [SwaggerResponse(StatusCodes.Status201Created, "Alocação criada com sucesso", typeof(AtivoUsuarioDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao alocar ativo")]
        public async Task<ActionResult<AtivoUsuarioDTO>> Alocar([FromBody] AtivoUsuarioDTO dto)
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
        [Route("v1/AtivoUsuario")]
        [SwaggerOperation(Summary = "Listar todas as alocações de Ativos para Usuários")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de alocações obtida com sucesso", typeof(IEnumerable<AtivoUsuarioDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoUsuarioDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/AtivoUsuario/{id:int}")]
        [SwaggerOperation(Summary = "Obter uma alocação de Ativo para Usuário específica pelo seu identificador")]
        [SwaggerResponse(StatusCodes.Status200OK, "Alocação encontrada", typeof(AtivoUsuarioDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Alocação não encontrada")]
        public async Task<ActionResult<AtivoUsuarioDTO>> ObterPorId(int id)
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
        [Route("v1/AtivoUsuario/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar uma alocação de Ativo para Usuário, permitindo definir data de término")]
        [SwaggerResponse(StatusCodes.Status200OK, "Alocação atualizada com sucesso", typeof(AtivoUsuarioDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Alocação não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao atualizar alocação")]
        public async Task<ActionResult<AtivoUsuarioDTO>> Atualizar(int id, [FromBody] AtivoUsuarioDTO dto)
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
        [Route("v1/AtivoUsuario/ativo/{ativoId:int}/encerrar")]
        [SwaggerOperation(Summary = "Encerrar a alocação ativa de um Ativo para Usuário, informando a data de término")]
        [SwaggerResponse(StatusCodes.Status200OK, "Alocação encerrada com sucesso", typeof(AtivoUsuarioDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Alocação não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao encerrar alocação")]
        public async Task<ActionResult<AtivoUsuarioDTO>> Encerrar(
            int ativoId,
            [FromQuery] DateTime dataFim
        )
        {
            try
            {
                var atualizado = await _service.Encerrar(ativoId, dataFim);
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
        [Route("v1/AtivoUsuario/{id:int}")]
        [SwaggerOperation(Summary = "Remover permanentemente uma alocação de Ativo para Usuário do sistema")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Alocação removida com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Alocação não encontrada")]
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


