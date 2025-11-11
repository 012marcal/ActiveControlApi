using ActiveControlApi.DTO.Usuario;
using ActiveControlApi.DTO.MappingExtensions;
using ActiveControlApi.Repositories;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace ActiveControlApi.Services.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _uow;

        public UsuarioService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<UsuarioDTO>> PegarTodos()
        {
            var list = await _uow.Usuario.GetAll();
            if (!list.Any()) return Enumerable.Empty<UsuarioDTO>();
            return list.ParaListaUsuarioDto();
        }

        public async Task<UsuarioDTO> PegarPorId(int id)
        {
            var entity = await _uow.Usuario.Get(u => u.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Usuario com id {id} não encontrado.");
            return entity.ParaUsuarioDto();
        }

        public async Task<UsuarioDTO> Criar(UsuarioDTO dto)
        {
            if (!CpfValido(dto.Cpf)) throw new InvalidOperationException("CPF inválido.");
            if (await CpfJaExisteAsync(dto.Cpf)) throw new InvalidOperationException("CPF já cadastrado.");
            if (await EmailJaExisteAsync(dto.Email)) throw new InvalidOperationException("Email já cadastrado.");

            var (hash, salt) = GerarSenhaHash(dto.Senha);

            var entity = new Models.Usuario
            {
                NomeCompleto = dto.NomeCompleto,
                Cpf = SomenteDigitos(dto.Cpf),
                Email = dto.Email,
                SenhaHash = hash,
                SenhaSalt = salt,
                Role = dto.Role
            };

            var created = _uow.Usuario.Create(entity);
            await _uow.CommitAsync();
            return created.ParaUsuarioDto();
        }

        public async Task<UsuarioDTO> Atualizar(int id, UsuarioDTO dto)
        {
            var entity = await _uow.Usuario.Get(u => u.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Usuario com id {id} não encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.NomeCompleto)) entity.NomeCompleto = dto.NomeCompleto;

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != entity.Email)
            {
                if (await EmailJaExisteAsync(dto.Email)) throw new InvalidOperationException("Email já cadastrado.");
                entity.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.Cpf) && SomenteDigitos(dto.Cpf) != entity.Cpf)
            {
                if (!CpfValido(dto.Cpf)) throw new InvalidOperationException("CPF inválido.");
                if (await CpfJaExisteAsync(dto.Cpf)) throw new InvalidOperationException("CPF já cadastrado.");
                entity.Cpf = SomenteDigitos(dto.Cpf);
            }

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                var (hash, salt) = GerarSenhaHash(dto.Senha);
                entity.SenhaHash = hash;
                entity.SenhaSalt = salt;
            }

            _uow.Usuario.Update(entity);
            await _uow.CommitAsync();
            return entity.ParaUsuarioDto();
        }

        public async Task<bool> Remover(int id)
        {
            var entity = await _uow.Usuario.Get(u => u.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Usuario com id {id} não encontrado.");

            var possuiVinculos = await _uow.AtivoUsuario.Any(au => au.UsuarioId == id);
            if (possuiVinculos) throw new InvalidOperationException("Usuário possui ativos vinculados.");

            _uow.Usuario.Delete(entity);
            var linhas = await _uow.CommitAsync();
            return linhas > 0;
        }

        public bool CpfValido(string cpf)
        {
            var digits = SomenteDigitos(cpf);
            if (digits.Length != 11) return false;
            if (new string(digits[0], 11) == digits) return false;

            int[] m1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] m2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string temp = digits.Substring(0, 9);
            int sum = 0;
            for (int i = 0; i < 9; i++) sum += (temp[i] - '0') * m1[i];
            int r = sum % 11;
            r = r < 2 ? 0 : 11 - r;
            temp += r.ToString();
            sum = 0;
            for (int i = 0; i < 10; i++) sum += (temp[i] - '0') * m2[i];
            r = sum % 11;
            r = r < 2 ? 0 : 11 - r;
            return digits.EndsWith(temp[9].ToString() + r.ToString());
        }

        public async Task<bool> CpfJaExisteAsync(string cpf)
        {
            var digits = SomenteDigitos(cpf);
            return await _uow.Usuario.Any(u => u.Cpf == digits);
        }

        public async Task<bool> EmailJaExisteAsync(string email)
        {
            return await _uow.Usuario.Any(u => u.Email == email);
        }

        private static (byte[] hash, byte[] salt) GerarSenhaHash(string senha)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);
            using var hmac = new HMACSHA256(salt);
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return (hash, salt);
        }

        private static string SomenteDigitos(string valor) => Regex.Replace(valor ?? string.Empty, @"\D", "");
    }
}


