namespace FaturamentoService.Models.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
