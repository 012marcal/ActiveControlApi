using ActiveControlApi.DTO.Solicitacao;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Services.Solicitacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Solicitacoes
{
    [ApiController]
    [Authorize(Roles = "Admin,SuperUser")]
    public class SolicitacaoController : ControllerBase
    {
        private readonly ISolicitacaoService _solicitacaoService;

        public SolicitacaoController(ISolicitacaoService solicitacaoService)
        {
            _solicitacaoService = solicitacaoService;
        }

        [HttpPost]
        [Route("v1/Solicitacao")]
        [SwaggerOperation("Adicionar uma Solicitação")]
        [SwaggerResponse(StatusCodes.Status201Created, "Solicitação criada", typeof(SolicitacaoDTO))]
        public async Task<ActionResult<SolicitacaoDTO>> Adicionar([FromBody] SolicitacaoDTO dto)
        {
            try
            {
                var criado = await _solicitacaoService.CriarSolicitacao(dto);
                return StatusCode(StatusCodes.Status201Created, criado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Solicitacao")]
        [SwaggerOperation("Listar Solicitações")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Solicitações", typeof(IEnumerable<SolicitacaoDTO>))]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> ObterTodos()
        {
            var itens = await _solicitacaoService.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/{id:int}")]
        [SwaggerOperation("Obter Solicitação por Id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Solicitação", typeof(SolicitacaoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Não encontrado")]
        public async Task<ActionResult<SolicitacaoDTO>> ObterPorId(int id)
        {
            try
            {
                var item = await _solicitacaoService.PegarPorId(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/Solicitacao/{id:int}")]
        [SwaggerOperation("Atualizar Solicitação")]
        public async Task<ActionResult<SolicitacaoDTO>> Atualizar(int id, [FromBody] SolicitacaoDTO dto)
        {
            try
            {
                var atualizado = await _solicitacaoService.AtualizarSolicitacao(id, dto);
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
        [Route("v1/Solicitacao/{id:int}")]
        [SwaggerOperation("Remover Solicitação")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var ok = await _solicitacaoService.RemoverSolicitacao(id);
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
        [Route("v1/Solicitacao/Status/{status}")]
        [SwaggerOperation("Buscar solicitações por status")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarPorStatus(StatusSolicitacao status)
        {
            var itens = await _solicitacaoService.BuscarPorStatus(status);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Tipo/{tipo}")]
        [SwaggerOperation("Buscar solicitações por tipo")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarPorTipo(TipoSolicitacao tipo)
        {
            var itens = await _solicitacaoService.BuscarPorTipo(tipo);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Prioridade/{prioridade}")]
        [SwaggerOperation("Buscar solicitações por prioridade")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarPorPrioridade(PrioridadeSolicitacao prioridade)
        {
            var itens = await _solicitacaoService.BuscarPorPrioridade(prioridade);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Usuario/{usuarioId}")]
        [SwaggerOperation("Buscar solicitações de um usuário solicitante")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarPorUsuario(int usuarioId)
        {
            var itens = await _solicitacaoService.BuscarPorUsuarioSolicitante(usuarioId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Responsavel/{usuarioId}")]
        [SwaggerOperation("Buscar solicitações atribuídas a um responsável")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarPorResponsavel(int usuarioId)
        {
            var itens = await _solicitacaoService.BuscarPorUsuarioResponsavel(usuarioId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Ativo/{ativoId}")]
        [SwaggerOperation("Buscar solicitações de um ativo")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarPorAtivo(int ativoId)
        {
            var itens = await _solicitacaoService.BuscarPorAtivo(ativoId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Atrasadas")]
        [SwaggerOperation("Buscar solicitações atrasadas")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarAtrasadas()
        {
            var itens = await _solicitacaoService.BuscarAtrasadas();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Periodo")]
        [SwaggerOperation("Buscar solicitações por período")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarPorPeriodo(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var itens = await _solicitacaoService.BuscarPorPeriodo(dataInicio, dataFim);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Minhas/{usuarioId}")]
        [SwaggerOperation("Buscar minhas solicitações")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarMinhasSolicitacoes(int usuarioId)
        {
            var itens = await _solicitacaoService.BuscarMinhasSolicitacoes(usuarioId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Atribuidas/{usuarioId}")]
        [SwaggerOperation("Buscar solicitações atribuídas a mim")]
        public async Task<ActionResult<IEnumerable<SolicitacaoDTO>>> BuscarSolicitacoesAtribuidas(int usuarioId)
        {
            var itens = await _solicitacaoService.BuscarSolicitacoesAtribuidas(usuarioId);
            return Ok(itens);
        }

        // Workflow
        [HttpPost]
        [Route("v1/Solicitacao/{id:int}/Atribuir")]
        [SwaggerOperation("Atribuir responsável à solicitação")]
        public async Task<ActionResult<SolicitacaoDTO>> AtribuirResponsavel(int id, [FromBody] int usuarioResponsavelId)
        {
            try
            {
                var atualizado = await _solicitacaoService.AtribuirResponsavel(id, usuarioResponsavelId);
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
        [Route("v1/Solicitacao/{id:int}/Iniciar")]
        [SwaggerOperation("Iniciar atendimento da solicitação")]
        public async Task<ActionResult<SolicitacaoDTO>> IniciarAtendimento(int id)
        {
            try
            {
                var atualizado = await _solicitacaoService.IniciarAtendimento(id);
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
        [Route("v1/Solicitacao/{id:int}/Finalizar")]
        [SwaggerOperation("Finalizar solicitação")]
        public async Task<ActionResult<SolicitacaoDTO>> Finalizar(int id, [FromBody] string? observacao = null)
        {
            try
            {
                var atualizado = await _solicitacaoService.Finalizar(id, observacao);
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
        [Route("v1/Solicitacao/{id:int}/Cancelar")]
        [SwaggerOperation("Cancelar solicitação")]
        public async Task<ActionResult<SolicitacaoDTO>> Cancelar(int id, [FromBody] string? motivo = null)
        {
            try
            {
                var atualizado = await _solicitacaoService.Cancelar(id, motivo);
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
        [Route("v1/Solicitacao/{id:int}/Prioridade")]
        [SwaggerOperation("Alterar prioridade da solicitação")]
        public async Task<ActionResult<SolicitacaoDTO>> AlterarPrioridade(int id, [FromBody] PrioridadeSolicitacao prioridade)
        {
            try
            {
                var atualizado = await _solicitacaoService.AlterarPrioridade(id, prioridade);
                return Ok(atualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Relatórios
        [HttpGet]
        [Route("v1/Solicitacao/Estatisticas")]
        [SwaggerOperation("Obter estatísticas de solicitações")]
        public async Task<ActionResult<Dictionary<string, object>>> ObterEstatisticas()
        {
            var stats = await _solicitacaoService.ObterEstatisticas();
            return Ok(stats);
        }

        [HttpGet]
        [Route("v1/Solicitacao/Estatisticas/Periodo")]
        [SwaggerOperation("Obter estatísticas por período")]
        public async Task<ActionResult<Dictionary<string, object>>> ObterEstatisticasPorPeriodo(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var stats = await _solicitacaoService.ObterEstatisticasPorPeriodo(dataInicio, dataFim);
            return Ok(stats);
        }
    }
}

