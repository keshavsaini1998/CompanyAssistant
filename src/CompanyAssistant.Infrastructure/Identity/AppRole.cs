using Microsoft.AspNetCore.Identity;

namespace CompanyAssistant.Infrastructure.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        protected AppRole() { }
        public AppRole(string roleName) : base(roleName)
        {
        }
    }
}
