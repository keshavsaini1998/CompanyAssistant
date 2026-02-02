using CompanyAssistant.Application.Interfaces;

namespace CompanyAssistant.Application.UseCases
{
    public record AskQuestionCommand(Guid UserId, Guid ProjectId, string Question);
    public class AskQuestionHandler
    {
        private readonly IIdentityService _identity;
        private readonly IVectorStore _vector;
        private readonly IChatService _chat;

        public AskQuestionHandler(
            IIdentityService identity,
            IVectorStore vector,
            IChatService chat)
        {
            _identity = identity;
            _vector = vector;
            _chat = chat;
        }

        public async Task<string> Handle(AskQuestionCommand cmd)
        {
            if (!await _identity.HasProjectAccess(cmd.UserId, cmd.ProjectId))
                throw new UnauthorizedAccessException();


            var docs = await _vector.SearchAsync(cmd.Question, cmd.ProjectId);


            var context = string.Join("\n", docs.Select(d => d.Content));


            var prompt = $"""
You are a company assistant.
Answer ONLY from the context.
If the answer is not present, say 'I don’t know'.


Context:
{context}


Question:
{cmd.Question}
""";


            return await _chat.AskAsync(prompt);
        }
    }
}
