using CompanyAssistant.Application.Vector;

namespace CompanyAssistant.Application.Interfaces
{
    public interface IVectorStore
    {
        Task StoreAsync(string content, Guid projectId);
        Task<List<VectorDocument>> SearchAsync(string query, Guid projectId);
    }
}
