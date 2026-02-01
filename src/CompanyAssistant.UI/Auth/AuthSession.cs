namespace CompanyAssistant.UI.Auth
{
    public class AuthSession
    {
        public string Token { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string[] Roles { get; set; } = [];
    }
}
