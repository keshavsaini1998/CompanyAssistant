namespace CompanyAssistant.Application.Interfaces
{
    public interface IChatService
    {
        Task<string> AskAsync(string prompt);
    }
}
