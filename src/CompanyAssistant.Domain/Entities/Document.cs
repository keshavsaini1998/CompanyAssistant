namespace CompanyAssistant.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; private set; }
        public string TenantId { get; private set; }
        public string Role { get; private set; }
        public string Title { get; private set; }

        private readonly List<DocumentChunk> _chunks = new();
        public IReadOnlyCollection<DocumentChunk> Chunks => _chunks;

        public Document(Guid id, string tenantId, string role, string title)
        {
            Id = id;
            TenantId = tenantId;
            Role = role;
            Title = title;
        }

        public void AddChunk(DocumentChunk chunk)
        {
            _chunks.Add(chunk);
        }
    }
}
