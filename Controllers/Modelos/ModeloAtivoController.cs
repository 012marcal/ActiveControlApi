using ActiveControlApi.DTO.ModeloAtivo;
using ActiveControlApi.Services.ModeloAtivo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Modelos
{
    [ApiController]
    [Authorize]
    public class ModeloAtivoController : ControllerBase
    {
        private readonly IModeloAtivoService _service;

        public ModeloAtivoController(IModeloAtivoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/ModeloAtivo")]
        [SwaggerOperation(Summary = "Criar um novo Modelo de Ativo no sistema")]
        [SwaggerResponse(StatusCodes.Status201Created, "Modelo criado com sucesso", typeof(ModeloAtivoDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao criar modelo")]
        public async Task<ActionResult<ModeloAtivoDTO>> Adicionar([FromBody] ModeloAtivoDTO dto)
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
        [Route("v1/ModeloAtivo")]
        [SwaggerOperation(Summary = "Listar todos os Modelos de Ativo cadastrados")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de modelos obtida com sucesso", typeof(IEnumerable<ModeloAtivoDTO>))]
        public async Task<ActionResult<IEnumerable<ModeloAtivoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/ModeloAtivo/{id:int}")]
        [SwaggerOperation(Summary = "Obter um Modelo de Ativo específico pelo seu identificador")]
        [SwaggerResponse(StatusCodes.Status200OK, "Modelo encontrado", typeof(ModeloAtivoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Modelo não encontrado")]
        public async Task<ActionResult<ModeloAtivoDTO>> ObterPorId(int id)
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
        [Route("v1/ModeloAtivo/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar informações de um Modelo de Ativo existente")]
        [SwaggerResponse(StatusCodes.Status200OK, "Modelo atualizado com sucesso", typeof(ModeloAtivoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Modelo não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao atualizar modelo")]
        public async Task<ActionResult<ModeloAtivoDTO>> Atualizar(int id, [FromBody] ModeloAtivoDTO dto)
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
        [Route("v1/ModeloAtivo/{id:int}")]
        [SwaggerOperation(Summary = "Remover um Modelo de Ativo do sistema")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Modelo removido com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Modelo não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao remover modelo")]
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


