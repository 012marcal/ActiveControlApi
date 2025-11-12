using ActiveControlApi.DTO.Empresas;
using ActiveControlApi.DTO.Departamento;
using ActiveControlApi.Services.Empresa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ActiveControlApi.Controllers.Empresas
{
    //[Route("v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmpresaController : ControllerBase
    {

        private readonly IEmpresaService _empresaService;

        public EmpresaController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;

        }

        [HttpPost]
        [Route("v1/Empresa")]
        [SwaggerOperation(Summary = "Adi��o de um(a) Empresa, Adi��o de um(a) Empresa e retornando o Id do objeto adicionado, logo n�o � necess�rio informar o id")]
        [SwaggerResponse(200, "O Id do objeto adicionado: ", typeof(int))]
        public async Task<ActionResult<EmpresaDTO>> Adicionar(EmpresaDTO empresaDTO)
        {
            try
            {
                var empresaCriada = await _empresaService.CriarEmpresa(empresaDTO);
                return StatusCode(StatusCodes.Status201Created, empresaCriada);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message );
            }


        }

        [HttpGet]
        [Route("v1/Empresa")]
        [SwaggerOperation(Summary = "Obter todas as Empresas cadastradas.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de empresas obtida com sucesso.", typeof(IEnumerable<EmpresaDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nenhuma empresa encontrada.")]
        public async Task<ActionResult<IEnumerable<EmpresaDTO>>> ObterTodas()
        {
            var empresas = await _empresaService.PegarTodas();

            if (!empresas.Any())
                return NotFound(new { message = "Nenhuma empresa encontrada." });

            return Ok(empresas);
        }

        [HttpGet]
        [Route("v1/Empresa/{id:int}")]
        [SwaggerOperation(Summary = "Obter uma Empresa pelo seu Id.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Empresa encontrada.", typeof(EmpresaDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empresa n�o encontrada.")]
        public async Task<ActionResult<EmpresaDTO>> ObterPorId(int id)
        {
            try
            {
                var empresa = await _empresaService.PegarPorId(id);
                return Ok(empresa);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("v1/Empresa/{id:int}")]
        [SwaggerOperation(Summary = "Atualizar uma Empresa existente")]
        [SwaggerResponse(StatusCodes.Status200OK, "Empresa atualizada com sucesso.", typeof(EmpresaDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empresa n�o encontrada.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao atualizar a empresa.")]
        public async Task<ActionResult<EmpresaDTO>> Atualizar(int id, [FromBody] EmpresaDTO empresaDTO)
        {
            try
            {
                var empresaAtualizada = await _empresaService.AtualizarEmpresa(id, empresaDTO);
                return Ok(empresaAtualizada);
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
        [Route("v1/Empresa/{id:int}")]
        [SwaggerOperation(Summary = "Remover uma Empresa existente pelo Id.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Empresa removida com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empresa n�o encontrada.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro ao remover a empresa.")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var sucesso = await _empresaService.RemoverEmpresa(id);

                if (!sucesso)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Erro ao remover a empresa." });

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Empresa/Buscar")]
        [SwaggerOperation(Summary = "Buscar Empresas por filtros: Raz�o Social, CNPJ, Cidade ou Estado.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Empresas encontradas.", typeof(IEnumerable<EmpresaDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nenhuma empresa encontrada com os filtros fornecidos.")]
        public async Task<ActionResult<IEnumerable<EmpresaDTO>>> Buscar([FromQuery] FiltroEmpresaDTO filtro)
        {
            var empresas = await _empresaService.BuscarEmpresas(filtro);

            if (!empresas.Any())
                return NotFound(new { message = "Nenhuma empresa encontrada com os filtros fornecidos." });

            return Ok(empresas);
        }

        [HttpGet]
        [Route("v1/Empresa/{id:int}/Departamentos")]
        [SwaggerOperation(Summary = "Obter todos os Departamentos de uma Empresa.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Departamentos encontrados.", typeof(IEnumerable<DepartamentoDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empresa n�o encontrada ou sem departamentos.")]
        public async Task<ActionResult<IEnumerable<DepartamentoDTO>>> ObterDepartamentosPorEmpresa(int id)
        {
            try
            {
                var departamentos = await _empresaService.ObterDepartamentosPorEmpresa(id);
                
                if (!departamentos.Any())
                    return NotFound(new { message = $"Nenhum departamento encontrado para a empresa com id {id}." });

                return Ok(departamentos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
