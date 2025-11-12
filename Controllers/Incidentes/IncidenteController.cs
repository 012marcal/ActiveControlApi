using ActiveControlApi.DTO.Incidente;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Services.Incidente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Incidentes
{
    [ApiController]
    [Authorize(Roles = "Admin,SuperUser")]
    public class IncidenteController : ControllerBase
    {
        private readonly IIncidenteService _incidenteService;

        public IncidenteController(IIncidenteService incidenteService)
        {
            _incidenteService = incidenteService;
        }

        [HttpPost]
        [Route("v1/Incidente")]
        [SwaggerOperation(Summary = "Adicionar um Incidente")]
        [SwaggerResponse(StatusCodes.Status201Created, "Incidente criado", typeof(IncidenteDTO))]
        public async Task<ActionResult<IncidenteDTO>> Adicionar([FromBody] IncidenteDTO dto)
        {
            try
            {
                var criado = await _incidenteService.CriarIncidente(dto);
                return StatusCode(StatusCodes.Status201Created, criado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Incidente")]
        [SwaggerOperation(Summary = "Listar Incidentes")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Incidentes", typeof(IEnumerable<IncidenteDTO>))]
        public async Task<ActionResult<IEnumerable<IncidenteDTO>>> ObterTodos()
        {
            var itens = await _incidenteService.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Incidente/{id:int}")]
        [SwaggerOperation(Summary = "Obter Incidente por Id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Incidente", typeof(IncidenteDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Não encontrado")]
        public async Task<ActionResult<IncidenteDTO>> ObterPorId(int id)
        {
            try
            {
                var item = await _incidenteService.PegarPorId(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/Incidente/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar Incidente")]
        public async Task<ActionResult<IncidenteDTO>> Atualizar(int id, [FromBody] IncidenteDTO dto)
        {
            try
            {
                var atualizado = await _incidenteService.AtualizarIncidente(id, dto);
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
        [Route("v1/Incidente/{id:int}")]
        [SwaggerOperation(Summary = "Remover Incidente")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var ok = await _incidenteService.RemoverIncidente(id);
                if (!ok)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao remover." });
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Filtros avançados
        [HttpGet]
        [Route("v1/Incidente/Status/{status}")]
        [SwaggerOperation(Summary = "Buscar incidentes por status")]
        public async Task<ActionResult<IEnumerable<IncidenteDTO>>> BuscarPorStatus(StatusIncidente status)
        {
            var itens = await _incidenteService.BuscarPorStatus(status);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Incidente/Severidade/{severidade}")]
        [SwaggerOperation(Summary = "Buscar incidentes por severidade")]
        public async Task<ActionResult<IEnumerable<IncidenteDTO>>> BuscarPorSeveridade(SeveridadeIncidente severidade)
        {
            var itens = await _incidenteService.BuscarPorSeveridade(severidade);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Incidente/Prioridade/{prioridade}")]
        [SwaggerOperation(Summary = "Buscar incidentes por prioridade")]
        public async Task<ActionResult<IEnumerable<IncidenteDTO>>> BuscarPorPrioridade(PrioridadeSolicitacao prioridade)
        {
            var itens = await _incidenteService.BuscarPorPrioridade(prioridade);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Incidente/Responsavel/{usuarioId}")]
        [SwaggerOperation(Summary = "Buscar incidentes atribuídos a um responsável")]
        public async Task<ActionResult<IEnumerable<IncidenteDTO>>> BuscarPorResponsavel(int usuarioId)
        {
            var itens = await _incidenteService.BuscarPorUsuarioResponsavel(usuarioId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Incidente/Abertos")]
        [SwaggerOperation(Summary = "Buscar incidentes abertos")]
        public async Task<ActionResult<IEnumerable<IncidenteDTO>>> BuscarAbertos()
        {
            var itens = await _incidenteService.BuscarAbertos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Incidente/Atrasados")]
        [SwaggerOperation(Summary = "Buscar incidentes atrasados")]
        public async Task<ActionResult<IEnumerable<IncidenteDTO>>> BuscarAtrasados()
        {
            var itens = await _incidenteService.BuscarAtrasados();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Incidente/Periodo")]
        [SwaggerOperation(Summary = "Buscar incidentes por período")]
        public async Task<ActionResult<IEnumerable<IncidenteDTO>>> BuscarPorPeriodo(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var itens = await _incidenteService.BuscarPorPeriodo(dataInicio, dataFim);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Incidente/Atribuidos/{usuarioId}")]
        [SwaggerOperation(Summary = "Buscar incidentes atribuídos a mim")]
        public async Task<ActionResult<IEnumerable<IncidenteDTO>>> BuscarIncidentesAtribuidos(int usuarioId)
        {
            var itens = await _incidenteService.BuscarIncidentesAtribuidos(usuarioId);
            return Ok(itens);
        }

        // Workflow
        [HttpPost]
        [Route("v1/Incidente/{id:int}/Atribuir")]
        [SwaggerOperation(Summary = "Atribuir responsável ao incidente")]
        public async Task<ActionResult<IncidenteDTO>> AtribuirResponsavel(int id, [FromBody] int usuarioResponsavelId)
        {
            try
            {
                var atualizado = await _incidenteService.AtribuirResponsavel(id, usuarioResponsavelId);
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

        [HttpPost]
        [Route("v1/Incidente/{id:int}/IniciarAnalise")]
        [SwaggerOperation(Summary = "Iniciar análise do incidente")]
        public async Task<ActionResult<IncidenteDTO>> IniciarAnalise(int id)
        {
            try
            {
                var atualizado = await _incidenteService.IniciarAnalise(id);
                return Ok(atualizado);
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
        [Route("v1/Incidente/{id:int}/IniciarResolucao")]
        [SwaggerOperation(Summary = "Iniciar resolução do incidente")]
        public async Task<ActionResult<IncidenteDTO>> IniciarResolucao(int id)
        {
            try
            {
                var atualizado = await _incidenteService.IniciarResolucao(id);
                return Ok(atualizado);
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
        [Route("v1/Incidente/{id:int}/Resolver")]
        [SwaggerOperation(Summary = "Resolver incidente")]
        public async Task<ActionResult<IncidenteDTO>> Resolver(int id, [FromBody] ResolverIncidenteRequest? request = null)
        {
            try
            {
                var atualizado = await _incidenteService.Resolver(id, request?.SolucaoAplicada, request?.CausaRaiz);
                return Ok(atualizado);
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
        [Route("v1/Incidente/{id:int}/Cancelar")]
        [SwaggerOperation(Summary = "Cancelar incidente")]
        public async Task<ActionResult<IncidenteDTO>> Cancelar(int id, [FromBody] string? motivo = null)
        {
            try
            {
                var atualizado = await _incidenteService.Cancelar(id, motivo);
                return Ok(atualizado);
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

        [HttpPut]
        [Route("v1/Incidente/{id:int}/Prioridade")]
        [SwaggerOperation(Summary = "Alterar prioridade do incidente")]
        public async Task<ActionResult<IncidenteDTO>> AlterarPrioridade(int id, [FromBody] PrioridadeSolicitacao prioridade)
        {
            try
            {
                var atualizado = await _incidenteService.AlterarPrioridade(id, prioridade);
                return Ok(atualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/Incidente/{id:int}/Severidade")]
        [SwaggerOperation(Summary = "Alterar severidade do incidente")]
        public async Task<ActionResult<IncidenteDTO>> AlterarSeveridade(int id, [FromBody] SeveridadeIncidente severidade)
        {
            try
            {
                var atualizado = await _incidenteService.AlterarSeveridade(id, severidade);
                return Ok(atualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Relatórios
        [HttpGet]
        [Route("v1/Incidente/Estatisticas")]
        [SwaggerOperation(Summary = "Obter estatísticas de incidentes")]
        public async Task<ActionResult<Dictionary<string, object>>> ObterEstatisticas()
        {
            var stats = await _incidenteService.ObterEstatisticas();
            return Ok(stats);
        }
    }

    public class ResolverIncidenteRequest
    {
        public string? SolucaoAplicada { get; set; }
        public string? CausaRaiz { get; set; }
    }
}

