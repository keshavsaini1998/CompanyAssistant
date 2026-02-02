using CompanyAssistant.Application.Vector;

namespace CompanyAssistant.Application.Interfaces
{
    public interface ISqlReader
    {
        Task<List<VectorDocument>> ReadAsync(Guid projectId);
    }
}
