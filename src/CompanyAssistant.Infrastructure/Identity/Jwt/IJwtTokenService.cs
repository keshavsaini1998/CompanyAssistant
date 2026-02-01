namespace CompanyAssistant.Infrastructure.Identity.Jwt
{
    public interface IJwtTokenService
    {
        string GenerateToken(AppUser user, IList<string> roles);
    }
}
