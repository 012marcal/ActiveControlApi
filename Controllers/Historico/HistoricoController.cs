using ActiveControlApi.DTO.Historico;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Services.Historico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Historico
{
    [ApiController]
    [Route("v1/[controller]")]
    [Authorize]
    public class HistoricoController : ControllerBase
    {
        private readonly IHistoricoService _historicoService;

        public HistoricoController(IHistoricoService historicoService)
        {
            _historicoService = historicoService;
        }

        [HttpGet]
        [SwaggerOperation("Obter todo o histórico de movimentações")]
        [SwaggerResponse(StatusCodes.Status200OK, "Histórico obtido com sucesso", typeof(IEnumerable<HistoricoMovimentacaoDTO>))]
        public async Task<ActionResult<IEnumerable<HistoricoMovimentacaoDTO>>> ObterTodos()
        {
            var historico = await _historicoService.ObterTodos();
            return Ok(historico);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Obter histórico por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Histórico obtido com sucesso", typeof(HistoricoMovimentacaoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Histórico não encontrado")]
        public async Task<ActionResult<HistoricoMovimentacaoDTO>> ObterPorId(int id)
        {
            try
            {
                var historico = await _historicoService.ObterPorId(id);
                return Ok(historico);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("Ativo/{ativoId}")]
        [SwaggerOperation("Obter histórico de movimentações de um ativo específico")]
        [SwaggerResponse(StatusCodes.Status200OK, "Histórico obtido com sucesso", typeof(IEnumerable<HistoricoMovimentacaoDTO>))]
        public async Task<ActionResult<IEnumerable<HistoricoMovimentacaoDTO>>> ObterPorAtivo(int ativoId)
        {
            var historico = await _historicoService.ObterHistoricoPorAtivo(ativoId);
            return Ok(historico);
        }

        [HttpGet("Usuario/{usuarioId}")]
        [SwaggerOperation("Obter histórico de movimentações de um usuário específico")]
        [SwaggerResponse(StatusCodes.Status200OK, "Histórico obtido com sucesso", typeof(IEnumerable<HistoricoMovimentacaoDTO>))]
        public async Task<ActionResult<IEnumerable<HistoricoMovimentacaoDTO>>> ObterPorUsuario(int usuarioId)
        {
            var historico = await _historicoService.ObterHistoricoPorUsuario(usuarioId);
            return Ok(historico);
        }

        [HttpGet("Tipo/{tipo}")]
        [SwaggerOperation("Obter histórico por tipo de movimentação")]
        [SwaggerResponse(StatusCodes.Status200OK, "Histórico obtido com sucesso", typeof(IEnumerable<HistoricoMovimentacaoDTO>))]
        public async Task<ActionResult<IEnumerable<HistoricoMovimentacaoDTO>>> ObterPorTipo(TipoMovimentacao tipo)
        {
            var historico = await _historicoService.ObterHistoricoPorTipo(tipo);
            return Ok(historico);
        }

        [HttpGet("Periodo")]
        [SwaggerOperation("Obter histórico por período")]
        [SwaggerResponse(StatusCodes.Status200OK, "Histórico obtido com sucesso", typeof(IEnumerable<HistoricoMovimentacaoDTO>))]
        public async Task<ActionResult<IEnumerable<HistoricoMovimentacaoDTO>>> ObterPorPeriodo(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var historico = await _historicoService.ObterHistoricoPorPeriodo(dataInicio, dataFim);
            return Ok(historico);
        }
    }
}


