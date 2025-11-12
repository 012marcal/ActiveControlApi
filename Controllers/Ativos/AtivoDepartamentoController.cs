using ActiveControlApi.DTO.AtivoDepartamento;
using ActiveControlApi.Services.AtivoDepartamento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Ativos
{
    [ApiController]
    [Authorize]
    public class AtivoDepartamentoController : ControllerBase
    {
        private readonly IAtivoDepartamentoService _service;

        public AtivoDepartamentoController(IAtivoDepartamentoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/AtivoDepartamento")]
        [SwaggerOperation(Summary = "Alocar um Ativo para um Departamento específico")]
        [SwaggerResponse(StatusCodes.Status201Created, "Alocação criada com sucesso", typeof(AtivoDepartamentoDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao alocar ativo")]
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
        [SwaggerOperation(Summary = "Listar todas as alocações de Ativos para Departamentos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de alocações obtida com sucesso", typeof(IEnumerable<AtivoDepartamentoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDepartamentoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/AtivoDepartamento/{id:int}")]
        [SwaggerOperation(Summary = "Obter uma alocação de Ativo para Departamento específica pelo seu identificador")]
        [SwaggerResponse(StatusCodes.Status200OK, "Alocação encontrada", typeof(AtivoDepartamentoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Alocação não encontrada")]
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
        [Route("v1/AtivoDepartamento/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar uma alocação de Ativo para Departamento, permitindo definir data de término")]
        [SwaggerResponse(StatusCodes.Status200OK, "Alocação atualizada com sucesso", typeof(AtivoDepartamentoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Alocação não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao atualizar alocação")]
        public async Task<ActionResult<AtivoDepartamentoDTO>> Atualizar(int id, [FromBody] AtivoDepartamentoDTO dto)
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
        [Route("v1/AtivoDepartamento/{id:int}/encerrar")]
        [SwaggerOperation(Summary = "Encerrar uma alocação de Ativo para Departamento, definindo a data de término como a data atual")]
        [SwaggerResponse(StatusCodes.Status200OK, "Alocação encerrada com sucesso", typeof(AtivoDepartamentoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Alocação não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao encerrar alocação")]
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
        [SwaggerOperation(Summary = "Remover permanentemente uma alocação de Ativo para Departamento do sistema")]
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


