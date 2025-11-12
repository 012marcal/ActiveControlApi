using ActiveControlApi.DTO.Manutencao;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Services.Manutencao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Manutencoes
{
    [ApiController]
    [Authorize]
    public class ManutencaoController : ControllerBase
    {
        private readonly IManutencaoService _service;

        public ManutencaoController(IManutencaoService service)
        {
            _service = service;
        }

        // ========== CRUD ==========

        [HttpPost]
        [Route("v1/Manutencao")]
        [SwaggerOperation(Summary = "Criar nova Manutenção")]
        [SwaggerResponse(StatusCodes.Status201Created, "Manutenção criada com sucesso.", typeof(ManutencaoDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao criar manutenção.")]
        public async Task<ActionResult<ManutencaoDTO>> Criar([FromBody] CriarManutencaoDTO dto)
        {
            try
            {
                var manutencao = await _service.Criar(dto);
                return StatusCode(StatusCodes.Status201Created, manutencao);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Manutencao")]
        [SwaggerOperation(Summary = "Listar todas as Manutenções")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de manutenções obtida com sucesso.", typeof(IEnumerable<ManutencaoDTO>))]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> ObterTodos()
        {
            var manutencoes = await _service.PegarTodos();
            return Ok(manutencoes);
        }

        [HttpGet]
        [Route("v1/Manutencao/{id:int}")]
        [SwaggerOperation(Summary = "Obter Manutenção por Id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenção encontrada.", typeof(ManutencaoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Manutenção não encontrada.")]
        public async Task<ActionResult<ManutencaoDTO>> ObterPorId(int id)
        {
            try
            {
                var manutencao = await _service.PegarPorId(id);
                return Ok(manutencao);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/Manutencao/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar Manutenção")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenção atualizada com sucesso.", typeof(ManutencaoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Manutenção não encontrada.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao atualizar manutenção.")]
        public async Task<ActionResult<ManutencaoDTO>> Atualizar(int id, [FromBody] AtualizarManutencaoDTO dto)
        {
            try
            {
                var manutencao = await _service.Atualizar(id, dto);
                return Ok(manutencao);
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

        [HttpDelete]
        [Route("v1/Manutencao/{id:int}")]
        [SwaggerOperation(Summary = "Remover Manutenção")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Manutenção removida com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Manutenção não encontrada.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao remover manutenção.")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var sucesso = await _service.Remover(id);
                if (!sucesso)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao remover manutenção." });
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

        // ========== Paginação e Filtros ==========

        [HttpGet]
        [Route("v1/Manutencao/Paginado")]
        [SwaggerOperation(Summary = "Obter Manutenções Paginadas com Filtros")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista paginada de manutenções.", typeof(object))]
        public async Task<ActionResult<object>> ObterPaginado(
            [FromQuery] int pagina = 1, 
            [FromQuery] int tamanhoPagina = 10, 
            [FromQuery] StatusManutencao? status = null,
            [FromQuery] TipoManutencao? tipo = null,
            [FromQuery] PrioridadeSolicitacao? prioridade = null,
            [FromQuery] int? ativoId = null,
            [FromQuery] int? usuarioResponsavelId = null,
            [FromQuery] int? solicitacaoId = null,
            [FromQuery] DateTime? dataInicio = null,
            [FromQuery] DateTime? dataFim = null,
            [FromQuery] bool? atrasadas = null)
        {
            var filtro = new FiltroManutencaoDTO
            {
                Status = status,
                Tipo = tipo,
                Prioridade = prioridade,
                AtivoId = ativoId,
                UsuarioResponsavelId = usuarioResponsavelId,
                SolicitacaoId = solicitacaoId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Atrasadas = atrasadas
            };

            var (itens, total) = await _service.PegarPaginado(pagina, tamanhoPagina, filtro);
            return Ok(new { total, pagina, tamanhoPagina, itens });
        }

        [HttpPost]
        [Route("v1/Manutencao/Buscar")]
        [SwaggerOperation(Summary = "Buscar Manutenções com Filtros Avançados")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenções encontradas.", typeof(IEnumerable<ManutencaoDTO>))]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarComFiltros([FromBody] FiltroManutencaoDTO filtro)
        {
            var manutencoes = await _service.BuscarComFiltros(filtro);
            return Ok(manutencoes);
        }

        // ========== Mudança de Status ==========

        [HttpPost]
        [Route("v1/Manutencao/{id:int}/Iniciar")]
        [SwaggerOperation(Summary = "Iniciar Manutenção")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenção iniciada com sucesso.", typeof(ManutencaoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Manutenção não encontrada.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao iniciar manutenção.")]
        public async Task<ActionResult<ManutencaoDTO>> IniciarManutencao(int id, [FromQuery] int? usuarioResponsavelId = null)
        {
            try
            {
                var manutencao = await _service.IniciarManutencao(id, usuarioResponsavelId);
                return Ok(manutencao);
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

        [HttpPost]
        [Route("v1/Manutencao/{id:int}/Concluir")]
        [SwaggerOperation(Summary = "Concluir Manutenção")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenção concluída com sucesso.", typeof(ManutencaoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Manutenção não encontrada.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao concluir manutenção.")]
        public async Task<ActionResult<ManutencaoDTO>> ConcluirManutencao(
            int id, 
            [FromQuery] string? solucaoAplicada = null, 
            [FromQuery] decimal? custoReal = null)
        {
            try
            {
                var manutencao = await _service.ConcluirManutencao(id, solucaoAplicada, custoReal);
                return Ok(manutencao);
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

        [HttpPost]
        [Route("v1/Manutencao/{id:int}/Cancelar")]
        [SwaggerOperation(Summary = "Cancelar Manutenção")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenção cancelada com sucesso.", typeof(ManutencaoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Manutenção não encontrada.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao cancelar manutenção.")]
        public async Task<ActionResult<ManutencaoDTO>> CancelarManutencao(int id, [FromQuery] string? motivo = null)
        {
            try
            {
                var manutencao = await _service.CancelarManutencao(id, motivo);
                return Ok(manutencao);
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

        // ========== Consultas Específicas ==========

        [HttpGet]
        [Route("v1/Manutencao/Ativo/{ativoId:int}")]
        [SwaggerOperation(Summary = "Buscar Manutenções por Ativo")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenções encontradas.", typeof(IEnumerable<ManutencaoDTO>))]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarPorAtivo(int ativoId)
        {
            var manutencoes = await _service.BuscarPorAtivo(ativoId);
            return Ok(manutencoes);
        }

        [HttpGet]
        [Route("v1/Manutencao/Usuario/{usuarioId:int}")]
        [SwaggerOperation(Summary = "Buscar Manutenções por Usuário Responsável")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenções encontradas.", typeof(IEnumerable<ManutencaoDTO>))]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarPorUsuarioResponsavel(int usuarioId)
        {
            var manutencoes = await _service.BuscarPorUsuarioResponsavel(usuarioId);
            return Ok(manutencoes);
        }

        [HttpGet]
        [Route("v1/Manutencao/Atrasadas")]
        [SwaggerOperation(Summary = "Buscar Manutenções Atrasadas")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenções atrasadas encontradas.", typeof(IEnumerable<ManutencaoDTO>))]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarAtrasadas()
        {
            var manutencoes = await _service.BuscarAtrasadas();
            return Ok(manutencoes);
        }

        [HttpGet]
        [Route("v1/Manutencao/Periodo")]
        [SwaggerOperation(Summary = "Buscar Manutenções por Período")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenções encontradas.", typeof(IEnumerable<ManutencaoDTO>))]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarPorPeriodo(
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim)
        {
            var manutencoes = await _service.BuscarPorPeriodo(dataInicio, dataFim);
            return Ok(manutencoes);
        }
    }
}

