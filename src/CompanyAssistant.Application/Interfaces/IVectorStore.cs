using CompanyAssistant.Application.Vector;

namespace CompanyAssistant.Application.Interfaces
{
    public interface IVectorStore
    {
        Task StoreAsync(IEnumerable<VectorDocument> docs);
        Task<List<VectorDocument>> SearchAsync(string query, Guid projectId);
    }
}
