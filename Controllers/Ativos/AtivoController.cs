using ActiveControlApi.DTO.Ativo;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Services.Ativo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Ativos
{
    [ApiController]
    [Authorize]
    public class AtivoController : ControllerBase
    {
        private readonly IAtivoService _ativoService;

        public AtivoController(IAtivoService ativoService)
        {
            _ativoService = ativoService;
        }

        [HttpPost]
        [Route("v1/Ativo")]
        [SwaggerOperation(Summary = "Adicionar um Ativo")]
        [SwaggerResponse(StatusCodes.Status201Created, "Ativo criado", typeof(AtivoDTO))]
        public async Task<ActionResult<AtivoDTO>> Adicionar([FromBody] AtivoDTO dto)
        {
            try
            {
                var criado = await _ativoService.CriarAtivo(dto);
                return StatusCode(StatusCodes.Status201Created, criado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Ativo")]
        [SwaggerOperation(Summary = "Listar Ativos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> ObterTodos()
        {
            var itens = await _ativoService.PegarTodos();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Ativo/Paginado")]
        [SwaggerOperation(Summary = "Listar Ativos de forma paginada para melhor performance em grandes volumes de dados")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista paginada de ativos obtida com sucesso", typeof(object))]
        public async Task<ActionResult<object>> ObterPaginado([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
        {
            var (itens, total) = await _ativoService.PegarPaginado(pagina, tamanhoPagina);
            return Ok(new { total, pagina, tamanhoPagina, itens });
        }

        [HttpGet]
        [Route("v1/Ativo/{id:int}")]
        [SwaggerOperation(Summary = "Obter Ativo por Id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Ativo", typeof(AtivoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Não encontrado")]
        public async Task<ActionResult<AtivoDTO>> ObterPorId(int id)
        {
            try
            {
                var item = await _ativoService.PegarPorId(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet]
        [Route("v1/Ativo/Completo/{id:int}")]
        [SwaggerOperation(Summary = "Obter Ativo por id completo com Modelo e Categoria")]
        public async Task<IActionResult> GetAtivoCompletoPorId(int id)
        {
            try
            {
                var ativo = await _ativoService.GetAtivoCompleto(id);
                return Ok(ativo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet]
        [Route("v1/Ativo/Completo")]
        [SwaggerOperation(Summary = "Listar todos Ativos completos com Modelo e Categoria")]
        public async Task<IActionResult> GetTodosAtivosCompletos()
        {
            var itens = await _ativoService.GetTodosAtivosCompletos();
            return Ok(itens);
        }

        [HttpPut]
        [Route("v1/Ativo/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar Ativo")]
        public async Task<ActionResult<AtivoDTO>> Atualizar(int id, [FromBody] AtivoDTO dto)
        {
            try
            {
                var atualizado = await _ativoService.AtualizarAtivo(id, dto);
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
        [Route("v1/Ativo/{id:int}")]
        [SwaggerOperation(Summary = "Remover Ativo")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var ok = await _ativoService.RemoverAtivo(id);
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

        // Consultas avançadas
        [HttpGet]
        [Route("v1/Ativo/status/{status}")]
        [SwaggerOperation(Summary = "Buscar Ativos por Status")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> BuscarPorStatus(statusAtivo status)
        {
            var itens = await _ativoService.BuscarPorStatus(status);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Ativo/categoria/{categoriaId:int}")]
        [SwaggerOperation(Summary = "Buscar Ativos por Categoria")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> BuscarPorCategoria(int categoriaId)
        {
            var itens = await _ativoService.BuscarPorCategoria(categoriaId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Ativo/modelo/{modeloId:int}")]
        [SwaggerOperation(Summary = "Buscar Ativos por Modelo")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> BuscarPorModelo(int modeloId)
        {
            var itens = await _ativoService.BuscarPorModelo(modeloId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Ativo/disponiveis")]
        [SwaggerOperation(Summary = "Buscar Ativos Disponíveis")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos Disponíveis", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> BuscarDisponiveis()
        {
            var itens = await _ativoService.BuscarDisponiveis();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Ativo/manutencao")]
        [SwaggerOperation(Summary = "Buscar Ativos em Manutenção")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos em Manutenção", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> BuscarEmManutencao()
        {
            var itens = await _ativoService.BuscarEmManutencao();
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Ativo/patrimonio/{numPatrimonio}")]
        [SwaggerOperation(Summary = "Buscar Ativo por Número de Patrimônio")]
        [SwaggerResponse(StatusCodes.Status200OK, "Ativo", typeof(AtivoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Não encontrado")]
        public async Task<ActionResult<AtivoDTO>> BuscarPorNumeroPatrimonio(string numPatrimonio)
        {
            try
            {
                var item = await _ativoService.BuscarPorNumeroPatrimonio(numPatrimonio);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Ativo/serie/{numSerie}")]
        [SwaggerOperation(Summary = "Buscar Ativo por Número de Série")]
        [SwaggerResponse(StatusCodes.Status200OK, "Ativo", typeof(AtivoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Não encontrado")]
        public async Task<ActionResult<AtivoDTO>> BuscarPorNumeroSerie(string numSerie)
        {
            try
            {
                var item = await _ativoService.BuscarPorNumeroSerie(numSerie);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Ativo/usuario/{usuarioId:int}")]
        [SwaggerOperation(Summary = "Buscar Ativos por Usuário")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> BuscarPorUsuario(int usuarioId)
        {
            var itens = await _ativoService.BuscarPorUsuario(usuarioId);
            return Ok(itens);
        }

        [HttpGet]
        [Route("v1/Ativo/departamento/{departamentoId:int}")]
        [SwaggerOperation(Summary = "Buscar Ativos por Departamento")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> BuscarPorDepartamento(int departamentoId)
        {
            var itens = await _ativoService.BuscarPorDepartamento(departamentoId);
            return Ok(itens);
        }

        // Depreciação
        [HttpGet]
        [Route("v1/Ativo/{id:int}/depreciacao")]
        [SwaggerOperation(Summary = "Calcular Depreciação do Ativo")]
        [SwaggerResponse(StatusCodes.Status200OK, "Valor Depreciado")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Não encontrado")]
        public async Task<ActionResult<object>> CalcularDepreciacao(int id)
        {
            try
            {
                var valorDepreciado = await _ativoService.CalcularDepreciacao(id);
                return Ok(new { AtivoId = id, ValorDepreciado = valorDepreciado });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Ativo/vencimento")]
        [SwaggerOperation(Summary = "Listar Ativos Próximos ao Vencimento da Vida Útil")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de Ativos", typeof(IEnumerable<AtivoDTO>))]
        public async Task<ActionResult<IEnumerable<AtivoDTO>>> ListarProximosVencimento([FromQuery] int mesesAntecedencia = 6)
        {
            var itens = await _ativoService.ListarProximosVencimento(mesesAntecedencia);
            return Ok(itens);
        }

        // Relatórios
        [HttpGet]
        [Route("v1/Ativo/estatisticas")]
        [SwaggerOperation(Summary = "Obter Estatísticas dos Ativos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estatísticas")]
        public async Task<ActionResult<object>> ObterEstatisticas()
        {
            var estatisticas = await _ativoService.ObterEstatisticas();
            return Ok(estatisticas);
        }
    }
}


