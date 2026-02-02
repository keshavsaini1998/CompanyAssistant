using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace CompanyAssistant.Infrastructure.Vector
{
    public class QdrantInitializer
    {
        private const string CollectionName = "companyassistant";
        private const int VectorSize = 768; // ⚠️ must match embedding size

        public static async Task EnsureCreated(IQdrantClient client)
        {
            var collections = await client.ListCollectionsAsync();

            if (!collections.Any(c => c == CollectionName))
            {
                await client.CreateCollectionAsync(
                    collectionName: CollectionName,
                    vectorsConfig: new VectorParams
                    {
                        Size = VectorSize,
                        Distance = Distance.Cosine
                    }
                );
            }
        }
    }
}
