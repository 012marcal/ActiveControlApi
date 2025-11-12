using ActiveControlApi.DTO.Devolucao;
using ActiveControlApi.Services.Devolucao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Devolucoes
{
    [ApiController]
    [Authorize(Roles = "Admin,SuperUser")]
    public class DevolucaoController : ControllerBase
    {
        private readonly IDevolucaoService _devolucaoService;

        public DevolucaoController(IDevolucaoService devolucaoService)
        {
            _devolucaoService = devolucaoService;
        }

        [HttpPost]
        [Route("v1/Devolucao")]
        [SwaggerOperation(Summary = "Adicionar uma Devolução")]
        [SwaggerResponse(StatusCodes.Status201Created, "Devolução criada", typeof(DevolucaoDTO))]
        public async Task<ActionResult<DevolucaoDTO>> Adicionar([FromBody] DevolucaoDTO dto)
        {
            try
            {
                var criado = await _devolucaoService.CriarDevolucao(dto);
                return StatusCode(StatusCodes.Status201Created, criado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Devolucao")]
        [SwaggerOperation(Summary = "Listar Devoluções")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Devoluções", typeof(IEnumerable<DevolucaoDTO>))]
        public async Task<ActionResult<IEnumerable<DevolucaoDTO>>> ObterTodos()
        {
            var itens = await _devolucaoService.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Devolucao/{id:int}")]
        [SwaggerOperation(Summary = "Obter Devolução por Id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Devolução", typeof(DevolucaoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Não encontrado")]
        public async Task<ActionResult<DevolucaoDTO>> ObterPorId(int id)
        {
            try
            {
                var item = await _devolucaoService.PegarPorId(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/Devolucao/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar Devolução")]
        public async Task<ActionResult<DevolucaoDTO>> Atualizar(int id, [FromBody] DevolucaoDTO dto)
        {
            try
            {
                var atualizado = await _devolucaoService.AtualizarDevolucao(id, dto);
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
        [Route("v1/Devolucao/{id:int}")]
        [SwaggerOperation(Summary = "Remover Devolução")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var ok = await _devolucaoService.RemoverDevolucao(id);
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

