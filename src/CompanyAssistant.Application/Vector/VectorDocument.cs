namespace CompanyAssistant.Application.Vector
{
    public class VectorDocument
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProjectId { get; set; } = default!;
        public string Entity { get; set; } = default!;
        public string Content { get; set; } = default!;
    }
}
