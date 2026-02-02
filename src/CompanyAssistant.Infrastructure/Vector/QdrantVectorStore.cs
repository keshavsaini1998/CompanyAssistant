using CompanyAssistant.Application.Interfaces;
using CompanyAssistant.Application.Vector;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace CompanyAssistant.Infrastructure.Vector
{
    public class QdrantVectorStore : IVectorStore
    {
        private readonly IEmbeddingService _embedding;
        private readonly IQdrantClient _client;
        private const string CollectionName = "companyassistant";

        public QdrantVectorStore(IEmbeddingService embedding, IQdrantClient client)
        {
            _embedding = embedding;
            _client = client;
        }


        public async Task StoreAsync(IEnumerable<VectorDocument> docs)
        {
            foreach (var d in docs)
            {
                var vector = await _embedding.EmbedAsync(d.Content);

                var point = new PointStruct
                {
                    Id = new PointId(Guid.Parse(d.Id)),
                    Vectors = vector,
                    Payload =
                    {
                        ["projectId"] = d.ProjectId,
                        ["entity"] = d.Entity,
                        ["text"] = d.Content
                    }
                };
                await _client.UpsertAsync(CollectionName, new[] { point });
            }
        }


        public async Task<List<VectorDocument>> SearchAsync(string query, Guid projectId)
        {
            var vector = await _embedding.EmbedAsync(query);

            var filter = new Filter
            {
                Must =
            {
                new Condition
                {
                    Field = new FieldCondition
                    {
                        Key = "projectId",
                        Match = new Match
                        {
                            Keyword = projectId.ToString()
                        }
                    }
                }
            }
            };

            var results = await _client.SearchAsync(
                CollectionName,
                vector,
                limit: 5,
                filter: filter
            );


            return results.Select(r => new VectorDocument
            {
                Id = r.Id?.Uuid.ToString(),
                ProjectId = projectId.ToString(),
                Entity = r.Payload.TryGetValue("entity", out var e) ? e.StringValue : null,
                Content = r.Payload.TryGetValue("text", out var c) ? c.StringValue : null
            }).ToList();
        }
    }
}
