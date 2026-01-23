namespace CompanyAssistant.Domain.Entities
{
    public class ProjectMembership
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Role { get; set; } = "";
    }
}
