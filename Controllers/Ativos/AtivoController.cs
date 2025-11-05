using ActiveControlApi.DTO.Ativo;
using ActiveControlApi.Services.Ativo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace ActiveControlApi.Controllers.Ativos
{
    [ApiController]
    public class AtivoController : ControllerBase
    {
        private readonly IAtivoService _ativoService;

        public AtivoController(IAtivoService ativoService)
        {
            _ativoService = ativoService;
        }

        [HttpPost]
        [Route("v1/Ativo")]
        [SwaggerOperation("Adicionar um Ativo")]
        [SwaggerResponse(StatusCodes.Status201Created, "Ativo criado", typeof(AtivoDTO))]
        public async Task<ActionResult<AtivoDTO>> Adicionar([FromBody] AtivoDTO dto)
        {
            try
            {
                var criado = await _ativoService.CriarAtivo(dto);
                return StatusCode(StatusCodes.Status201Created, criado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Ativo")]
        [SwaggerOperation("Listar Ativos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> ObterTodos()
        {
            var itens = await _ativoService.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Ativo/{id:int}")]
        [SwaggerOperation("Obter Ativo por Id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Ativo", typeof(AtivoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Não encontrado")]
        public async Task<ActionResult<AtivoDTO>> ObterPorId(int id)
        {
            try
            {
                var item = await _ativoService.PegarPorId(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/Ativo/{id:int}")]
        [SwaggerOperation("Atualizar Ativo")]
        public async Task<ActionResult<AtivoDTO>> Atualizar(int id, [FromBody] AtivoDTO dto)
        {
            try
            {
                var atualizado = await _ativoService.AtualizarAtivo(id, dto);
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
        [Route("v1/Ativo/{id:int}")]
        [SwaggerOperation("Remover Ativo")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var ok = await _ativoService.RemoverAtivo(id);
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





