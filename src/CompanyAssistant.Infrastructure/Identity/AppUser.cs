using Microsoft.AspNetCore.Identity;

namespace CompanyAssistant.Infrastructure.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }
        public Guid TenantId { get; set; }
        public string? TenantName { get; set; }
    }
}
