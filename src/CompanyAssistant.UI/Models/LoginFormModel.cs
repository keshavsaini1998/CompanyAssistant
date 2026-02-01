namespace CompanyAssistant.UI.Models
{
    public class LoginFormModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
