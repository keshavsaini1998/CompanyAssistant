using Microsoft.AspNetCore.Identity;

namespace CompanyAssistant.Infrastructure.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public Guid TenantId { get; set; }
    }
}
