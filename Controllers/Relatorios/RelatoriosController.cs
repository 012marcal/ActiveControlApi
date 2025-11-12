using ActiveControlApi.Services.Relatorios;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Relatorios
{
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRelatorioService _service;

        public RelatoriosController(IRelatorioService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("v1/Relatorios/Ativos/Quantidade")]
        [SwaggerOperation(Summary = "Relatório de Quantidade de Ativos: Exibe o total de ativos cadastrados, organizados por categoria ou setor")]
        [SwaggerResponse(StatusCodes.Status200OK, "Relatório gerado com sucesso", typeof(object))]
        public async Task<ActionResult<object>> QuantidadeAtivos([FromQuery] string groupBy = "categoria")
        {
            var dados = await _service.QuantidadeAtivos(groupBy);
            return Ok(dados);
        }

        [HttpGet]
        [Route("v1/Relatorios/Ativos/PorProfissional")]
        [SwaggerOperation(Summary = "Relatório de Ativos por Profissional: Apresenta quais ativos estão sob a responsabilidade de cada funcionário")]
        [SwaggerResponse(StatusCodes.Status200OK, "Relatório gerado com sucesso", typeof(IEnumerable<object>))]
        public async Task<ActionResult<IEnumerable<object>>> AtivosPorProfissional()
        {
            var dados = await _service.AtivosPorProfissional();
            return Ok(dados);
        }

        [HttpGet]
        [Route("v1/Relatorios/Usuarios")]
        [SwaggerOperation(Summary = "Relatório de Funcionários: Lista de usuários ativos no sistema, com suas respectivas permissões e histórico de solicitações")]
        [SwaggerResponse(StatusCodes.Status200OK, "Relatório gerado com sucesso", typeof(IEnumerable<object>))]
        public async Task<ActionResult<IEnumerable<object>>> Usuarios()
        {
            var dados = await _service.UsuariosAtivos();
            return Ok(dados);
        }

        [HttpGet]
        [Route("v1/Relatorios/Ativos/Custos")]
        [SwaggerOperation(Summary = "Relatório de Custo por Ativo: Detalha os custos associados a cada ativo, incluindo aquisição, manutenção e depreciação")]
        [SwaggerResponse(StatusCodes.Status200OK, "Relatório gerado com sucesso", typeof(IEnumerable<object>))]
        public async Task<ActionResult<IEnumerable<object>>> CustoPorAtivo()
        {
            var dados = await _service.CustoPorAtivo();
            return Ok(dados);
        }

        [HttpGet]
        [Route("v1/Relatorios/Manutencoes/Preventivas")]
        [SwaggerOperation(Summary = "Relatório de Manutenção Preventiva: Informa os ativos que possuem manutenções programadas, suas datas e status de execução")]
        [SwaggerResponse(StatusCodes.Status200OK, "Relatório gerado com sucesso", typeof(IEnumerable<object>))]
        public async Task<ActionResult<IEnumerable<object>>> ManutencaoPreventiva()
        {
            var dados = await _service.ManutencaoPreventiva();
            return Ok(dados);
        }
    }
}






