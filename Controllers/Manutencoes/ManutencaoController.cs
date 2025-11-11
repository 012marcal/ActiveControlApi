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
    [Authorize(Roles = "Admin,SuperUser")]
    public class ManutencaoController : ControllerBase
    {
        private readonly IManutencaoService _manutencaoService;

        public ManutencaoController(IManutencaoService manutencaoService)
        {
            _manutencaoService = manutencaoService;
        }

        [HttpPost]
        [Route("v1/Manutencao")]
        [SwaggerOperation("Adicionar uma Manutenção")]
        [SwaggerResponse(StatusCodes.Status201Created, "Manutenção criada", typeof(ManutencaoDTO))]
        public async Task<ActionResult<ManutencaoDTO>> Adicionar([FromBody] ManutencaoDTO dto)
        {
            try
            {
                var criado = await _manutencaoService.CriarManutencao(dto);
                return StatusCode(StatusCodes.Status201Created, criado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Manutencao")]
        [SwaggerOperation("Listar Manutenções")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Manutenções", typeof(IEnumerable<ManutencaoDTO>))]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> ObterTodos()
        {
            var itens = await _manutencaoService.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Manutencao/{id:int}")]
        [SwaggerOperation("Obter Manutenção por Id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Manutenção", typeof(ManutencaoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Não encontrado")]
        public async Task<ActionResult<ManutencaoDTO>> ObterPorId(int id)
        {
            try
            {
                var item = await _manutencaoService.PegarPorId(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/Manutencao/{id:int}")]
        [SwaggerOperation("Atualizar Manutenção")]
        public async Task<ActionResult<ManutencaoDTO>> Atualizar(int id, [FromBody] ManutencaoDTO dto)
        {
            try
            {
                var atualizado = await _manutencaoService.AtualizarManutencao(id, dto);
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
        [Route("v1/Manutencao/{id:int}")]
        [SwaggerOperation("Remover Manutenção")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var ok = await _manutencaoService.RemoverManutencao(id);
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
        [Route("v1/Manutencao/Status/{status}")]
        [SwaggerOperation("Buscar manutenções por status")]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarPorStatus(StatusManutencao status)
        {
            var itens = await _manutencaoService.BuscarPorStatus(status);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Manutencao/Tipo/{tipo}")]
        [SwaggerOperation("Buscar manutenções por tipo")]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarPorTipo(TipoManutencao tipo)
        {
            var itens = await _manutencaoService.BuscarPorTipo(tipo);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Manutencao/Prioridade/{prioridade}")]
        [SwaggerOperation("Buscar manutenções por prioridade")]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarPorPrioridade(PrioridadeSolicitacao prioridade)
        {
            var itens = await _manutencaoService.BuscarPorPrioridade(prioridade);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Manutencao/Responsavel/{usuarioId}")]
        [SwaggerOperation("Buscar manutenções atribuídas a um responsável")]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarPorResponsavel(int usuarioId)
        {
            var itens = await _manutencaoService.BuscarPorUsuarioResponsavel(usuarioId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Manutencao/Atrasadas")]
        [SwaggerOperation("Buscar manutenções atrasadas")]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarAtrasadas()
        {
            var itens = await _manutencaoService.BuscarAtrasadas();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Manutencao/Agendadas")]
        [SwaggerOperation("Buscar manutenções agendadas")]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarAgendadas()
        {
            var itens = await _manutencaoService.BuscarAgendadas();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Manutencao/Periodo")]
        [SwaggerOperation("Buscar manutenções por período")]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarPorPeriodo(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var itens = await _manutencaoService.BuscarPorPeriodo(dataInicio, dataFim);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Manutencao/Atribuidas/{usuarioId}")]
        [SwaggerOperation("Buscar manutenções atribuídas a mim")]
        public async Task<ActionResult<IEnumerable<ManutencaoDTO>>> BuscarManutencoesAtribuidas(int usuarioId)
        {
            var itens = await _manutencaoService.BuscarManutencoesAtribuidas(usuarioId);
            return Ok(itens);
        }

        // Workflow
        [HttpPost]
        [Route("v1/Manutencao/{id:int}/Atribuir")]
        [SwaggerOperation("Atribuir responsável à manutenção")]
        public async Task<ActionResult<ManutencaoDTO>> AtribuirResponsavel(int id, [FromBody] int usuarioResponsavelId)
        {
            try
            {
                var atualizado = await _manutencaoService.AtribuirResponsavel(id, usuarioResponsavelId);
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
        [Route("v1/Manutencao/{id:int}/Agendar")]
        [SwaggerOperation("Agendar manutenção")]
        public async Task<ActionResult<ManutencaoDTO>> Agendar(int id, [FromBody] DateTime dataAgendada)
        {
            try
            {
                var atualizado = await _manutencaoService.Agendar(id, dataAgendada);
                return Ok(atualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("v1/Manutencao/{id:int}/Iniciar")]
        [SwaggerOperation("Iniciar manutenção")]
        public async Task<ActionResult<ManutencaoDTO>> Iniciar(int id)
        {
            try
            {
                var atualizado = await _manutencaoService.Iniciar(id);
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
        [Route("v1/Manutencao/{id:int}/Concluir")]
        [SwaggerOperation("Concluir manutenção")]
        public async Task<ActionResult<ManutencaoDTO>> Concluir(int id, [FromBody] ConcluirManutencaoRequest? request = null)
        {
            try
            {
                var atualizado = await _manutencaoService.Concluir(id, request?.SolucaoAplicada, request?.CustoReal);
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
        [Route("v1/Manutencao/{id:int}/Cancelar")]
        [SwaggerOperation("Cancelar manutenção")]
        public async Task<ActionResult<ManutencaoDTO>> Cancelar(int id, [FromBody] string? motivo = null)
        {
            try
            {
                var atualizado = await _manutencaoService.Cancelar(id, motivo);
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
        [Route("v1/Manutencao/{id:int}/Prioridade")]
        [SwaggerOperation("Alterar prioridade da manutenção")]
        public async Task<ActionResult<ManutencaoDTO>> AlterarPrioridade(int id, [FromBody] PrioridadeSolicitacao prioridade)
        {
            try
            {
                var atualizado = await _manutencaoService.AlterarPrioridade(id, prioridade);
                return Ok(atualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Relatórios
        [HttpGet]
        [Route("v1/Manutencao/Estatisticas")]
        [SwaggerOperation("Obter estatísticas de manutenções")]
        public async Task<ActionResult<Dictionary<string, object>>> ObterEstatisticas()
        {
            var stats = await _manutencaoService.ObterEstatisticas();
            return Ok(stats);
        }
    }

    public class ConcluirManutencaoRequest
    {
        public string? SolucaoAplicada { get; set; }
        public decimal? CustoReal { get; set; }
    }
}

