using ActiveControlApi.DTO.Auth;
using ActiveControlApi.Models.Enums;
using ActiveControlApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ActiveControlApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow;
            _configuration = configuration;
        }

        public async Task<TokenDTO> LoginAsync(LoginDTO loginDTO)
        {
            var usuario = await _uow.Usuario.Get(u => u.Email == loginDTO.Email);
            if (usuario == null)
                throw new UnauthorizedAccessException("Email ou senha inválidos.");

            if (!VerificarSenha(loginDTO.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                throw new UnauthorizedAccessException("Email ou senha inválidos.");

            var token = GerarToken(usuario);
            usuario.TokenDataCriacao = DateTime.UtcNow;
            _uow.Usuario.Update(usuario);
            await _uow.CommitAsync();

            return new TokenDTO
            {
                Token = token,
                ExpiraEm = DateTime.UtcNow.AddHours(8), // Token expira em 8 horas
                UsuarioId = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                Email = usuario.Email,
                Role = usuario.Role.ToString()
            };
        }

        public async Task<bool> ValidarTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada"));

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool VerificarSenha(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            using var hmac = new HMACSHA256(senhaSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return computedHash.SequenceEqual(senhaHash);
        }

        private string GerarToken(Models.Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada"));
            var expires = DateTime.UtcNow.AddHours(8);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NomeCompleto),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}



