using CompanyAssistant.Domain.Entities;
using CompanyAssistant.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyAssistant.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<UserProject> UserProjects => Set<UserProject>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<ProjectMembership>()
            //    .HasKey(x => new { x.UserId, x.ProjectId });
        }
    }

}
