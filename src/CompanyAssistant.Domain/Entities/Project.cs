namespace CompanyAssistant.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public Guid TenantId { get; set; }
    }
}
