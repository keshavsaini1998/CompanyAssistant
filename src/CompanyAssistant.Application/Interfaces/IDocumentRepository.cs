using CompanyAssistant.Domain.Entities;

namespace CompanyAssistant.Application.Interfaces
{
    public interface IDocumentRepository
    {
        Task AddAsync(Document document);
        Task<List<string>> GetAuthorizedChunksAsync(
            List<Guid> chunkIds,
            string role);
    }
}
