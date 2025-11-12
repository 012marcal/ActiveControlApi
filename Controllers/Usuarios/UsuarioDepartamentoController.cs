using ActiveControlApi.DTO.UsuarioDepartamento;
using ActiveControlApi.Services.UsuarioDepartamento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Usuarios
{
    [ApiController]
    [Authorize]
    public class UsuarioDepartamentoController : ControllerBase
    {
        private readonly IUsuarioDepartamentoService _service;

        public UsuarioDepartamentoController(IUsuarioDepartamentoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("v1/UsuarioDepartamento")]
        [SwaggerOperation(Summary = "Atribuir Usuário a Departamento")]
        public async Task<ActionResult<UsuarioDepartamentoDTO>> Atribuir([FromBody] CriarUsuarioDepartamentoDTO dto)
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
        [Route("v1/UsuarioDepartamento")]
        [SwaggerOperation(Summary = "Listar Todas as Atribuições de Departamento")]
        public async Task<ActionResult<IEnumerable<UsuarioDepartamentoDTO>>> ObterTodos()
        {
            var itens = await _service.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/UsuarioDepartamento/{id:int}")]
        [SwaggerOperation(Summary = "Obter Atribuição de Departamento por Id")]
        public async Task<ActionResult<UsuarioDepartamentoDTO>> ObterPorId(int id)
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
        [Route("v1/UsuarioDepartamento/usuario/{usuarioId:int}")]
        [SwaggerOperation(Summary = "Obter Departamentos de um Usuário")]
        public async Task<ActionResult<IEnumerable<UsuarioDepartamentoDTO>>> ObterPorUsuario(int usuarioId)
        {
            var itens = await _service.PegarPorUsuario(usuarioId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/UsuarioDepartamento/departamento/{departamentoId:int}")]
        [SwaggerOperation(Summary = "Obter Usuários de um Departamento")]
        public async Task<ActionResult<IEnumerable<UsuarioDepartamentoDTO>>> ObterPorDepartamento(int departamentoId)
        {
            var itens = await _service.PegarPorDepartamento(departamentoId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/UsuarioDepartamento/empresa/{empresaId:int}")]
        [SwaggerOperation(Summary = "Obter Atribuições por Empresa")]
        public async Task<ActionResult<IEnumerable<UsuarioDepartamentoDTO>>> ObterPorEmpresa(int empresaId)
        {
            var itens = await _service.PegarPorEmpresa(empresaId);
            return Ok(itens);
        }

        [HttpPut]
        [Route("v1/UsuarioDepartamento/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar Atribuição de Departamento")]
        public async Task<ActionResult<UsuarioDepartamentoDTO>> Atualizar(int id, [FromBody] AtualizarUsuarioDepartamentoDTO dto)
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
        [Route("v1/UsuarioDepartamento/{id:int}/encerrar")]
        [SwaggerOperation(Summary = "Encerrar Atribuição de Departamento")]
        public async Task<ActionResult<UsuarioDepartamentoDTO>> Encerrar(int id)
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
        [Route("v1/UsuarioDepartamento/{id:int}")]
        [SwaggerOperation(Summary = "Remover Atribuição de Departamento")]
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


