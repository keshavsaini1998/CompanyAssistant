namespace CompanyAssistant.Application.Interfaces
{
    public interface IVectorStore
    {
        Task StoreAsync(Guid chunkId, float[] vector, string tenantId);
        Task<List<Guid>> SearchAsync(float[] vector, string tenantId, int topK);
    }
}
