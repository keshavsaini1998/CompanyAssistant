using CompanyAssistant.Application.Interfaces;
using CompanyAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyAssistant.Infrastructure.Db
{
    public class EfDocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _db;

        public EfDocumentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Document document)
        {
            // Add document
            _db.Documents.Add(document);

            // Add chunks
            if (document.Chunks.Any())
            {
                _db.Chunks.AddRange(document.Chunks);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<List<string>> GetAuthorizedChunksAsync(List<Guid> chunkIds, string role)
        {
            // Join chunks with documents, filter by role
            var chunks = await _db.Chunks
                .Where(c => chunkIds.Contains(c.Id))
                .Join(_db.Documents,
                    c => c.DocumentId,
                    d => d.Id,
                    (c, d) => new { c.Content, d.Role })
                .Where(x => x.Role == role)  // Role-based ACL
                .Select(x => x.Content)
                .ToListAsync();

            return chunks;
        }
    }
}
