using CompanyAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyAssistant.Infrastructure.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<DocumentChunk> Chunks => Set<DocumentChunk>();
    }

}
