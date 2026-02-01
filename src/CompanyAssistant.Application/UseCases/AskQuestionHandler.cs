using CompanyAssistant.Application.Interfaces;

namespace CompanyAssistant.Application.UseCases
{
    public record AskQuestionQuery(
    string Question,
    string TenantId,
    string Role);
    public class AskQuestionHandler
    {
        private readonly IEmbeddingService _embed;
        private readonly IVectorStore _vector;
        private readonly IDocumentRepository _repo;
        private readonly IChatService _chat;

        public AskQuestionHandler(
            IEmbeddingService embed,
            IVectorStore vector,
            IDocumentRepository repo,
            IChatService chat)
        {
            _embed = embed;
            _vector = vector;
            _repo = repo;
            _chat = chat;
        }

//        public async Task<string> HandleAsync(AskQuestionQuery query)
//        {
//            var qVector = await _embed.EmbedAsync(query.Question);

//            var chunkIds = await _vector.SearchAsync(
//                qVector,
//                query.TenantId,
//                topK: 5);

//            var chunks = await _repo.GetAuthorizedChunksAsync(
//                chunkIds,
//                query.Role);

//            if (!chunks.Any())
//                return "I don't know";

//            var prompt = $"""
//You are a company assistant.
//Answer ONLY from the context below.

//Context:
//{string.Join("\n---\n", chunks)}

//Question:
//{query.Question}
//""";

//            return await _chat.AskAsync(prompt);
//        }
    }
}
