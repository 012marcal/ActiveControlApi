using ActiveControlApi.DTO.CategoriaAtivo;
using ActiveControlApi.Services.CategoriaAtivo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Categorias
{
    [ApiController]
    [Authorize]
    public class CategoriaAtivoController : ControllerBase
    {
        private readonly ICategoriaAtivoService _service;

        public CategoriaAtivoController(ICategoriaAtivoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/CategoriaAtivo")]
        [SwaggerOperation(Summary = "Criar uma nova Categoria de Ativo no sistema")]
        [SwaggerResponse(StatusCodes.Status201Created, "Categoria criada com sucesso", typeof(CategoriaAtivoDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao criar categoria")]
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
        [SwaggerOperation(Summary = "Listar todas as Categorias de Ativo cadastradas")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de categorias obtida com sucesso", typeof(IEnumerable<CategoriaAtivoDTO>))]
        public async Task<ActionResult<IEnumerable<CategoriaAtivoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodas();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/CategoriaAtivo/{id:int}")]
        [SwaggerOperation(Summary = "Obter uma Categoria de Ativo específica pelo seu identificador")]
        [SwaggerResponse(StatusCodes.Status200OK, "Categoria encontrada", typeof(CategoriaAtivoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Categoria não encontrada")]
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
        [SwaggerOperation(Summary = "Atualizar informações de uma Categoria de Ativo existente")]
        [SwaggerResponse(StatusCodes.Status200OK, "Categoria atualizada com sucesso", typeof(CategoriaAtivoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Categoria não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao atualizar categoria")]
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
        [SwaggerOperation(Summary = "Remover uma Categoria de Ativo do sistema")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Categoria removida com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Categoria não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao remover categoria")]
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


