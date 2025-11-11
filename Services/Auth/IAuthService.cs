using ActiveControlApi.DTO.Auth;

namespace ActiveControlApi.Services.Auth
{
    public interface IAuthService
    {
        Task<TokenDTO> LoginAsync(LoginDTO loginDTO);
        Task<bool> ValidarTokenAsync(string token);
    }
}



