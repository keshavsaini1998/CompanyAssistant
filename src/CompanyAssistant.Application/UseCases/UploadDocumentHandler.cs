using CompanyAssistant.Application.Interfaces;
using CompanyAssistant.Domain.Entities;

namespace CompanyAssistant.Application.UseCases
{
    public record UploadDocumentCommand(string TenantId,
    string Role,
    string Title,
    string Content);
    public class UploadDocumentHandler
    {
        private readonly IDocumentRepository _repo;
        private readonly IEmbeddingService _embed;
        private readonly IVectorStore _vector;

        public UploadDocumentHandler(
            IDocumentRepository repo,
            IEmbeddingService embed,
            IVectorStore vector)
        {
            _repo = repo;
            _embed = embed;
            _vector = vector;
        }

        //public async Task HandleAsync(UploadDocumentCommand cmd)
        //{
        //    var document = new Document(
        //        Guid.NewGuid(),
        //        cmd.TenantId,
        //        cmd.Role,
        //        cmd.Title);

        //    var chunks = cmd.Content.Chunk(500);

        //    foreach (var chunkText in chunks)
        //    {
        //        var chunk = new DocumentChunk(
        //            Guid.NewGuid(),
        //            document.Id,
        //            new string(chunkText));

        //        document.AddChunk(chunk);

        //        var embedding = await _embed.EmbedAsync(chunk.Content);
        //        await _vector.StoreAsync(chunk.Id, embedding, cmd.TenantId);
        //    }

        //    await _repo.AddAsync(document);
        //}
    }
}
