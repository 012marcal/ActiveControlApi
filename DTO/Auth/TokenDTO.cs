namespace ActiveControlApi.DTO.Auth
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
        public int UsuarioId { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}



