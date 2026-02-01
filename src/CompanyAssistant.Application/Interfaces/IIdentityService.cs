namespace CompanyAssistant.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Guid?> ValidateUserAsync(string username, string password);
        Task<string> GenerateJwtAsync(Guid userId);
    }
}
