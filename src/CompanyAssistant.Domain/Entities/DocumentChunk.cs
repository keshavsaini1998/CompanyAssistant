namespace CompanyAssistant.Domain.Entities
{
    public class DocumentChunk
    {
        public Guid Id { get; private set; }
        public Guid DocumentId { get; private set; }
        public string Content { get; private set; }

        public DocumentChunk(Guid id, Guid documentId, string content)
        {
            Id = id;
            DocumentId = documentId;
            Content = content;
        }
    }
}
