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


        public async Task StoreAsync(string content, Guid projectId)
        {
            var vector = await _embedding.EmbedAsync(content);

            var point = new PointStruct
            {
                Id = new PointId
                {
                    Uuid = Guid.NewGuid().ToString()
                },
                Vectors = vector,
                Payload =
                {
                    ["content"] = content,
                    ["projectId"] = projectId.ToString()
                }
            };

            await _client.UpsertAsync(CollectionName, new[] { point });
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
                Content = r.Payload["content"].StringValue,
                ProjectId = projectId
            }).ToList();
        }
    }
}
