using ActiveControlApi.DTO.Auth;
using ActiveControlApi.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace ActiveControlApi.Controllers.Auth
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("v1/Auth/Login")]
        [SwaggerOperation(Summary = "Realizar Login")]
        [SwaggerResponse(StatusCodes.Status200OK, "Login realizado com sucesso", typeof(TokenDTO))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Credenciais inválidas")]
        public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDTO);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("v1/Auth/me")]
        [SwaggerOperation(Summary = "Retornar informações de usuário logado")]
        public IActionResult BuscarUsuarioLogado()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nome = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;


            if(userId == null || email==null)
            {
                return Unauthorized(new
                {
                    Mensagem = "Usuário Invalido"
                });
            }

            return Ok(new
            {
                UserId = userId,
                NomeCompleto = nome,
                Email = email,
                Role = role,

            });
        }
    }
}



