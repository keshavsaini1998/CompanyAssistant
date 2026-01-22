using CompanyAssistant.Application.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace CompanyAssistant.Infrastructure.Vector
{
    public class QdrantVectorStore : IVectorStore
    {
        private readonly HttpClient _http;
        public QdrantVectorStore(HttpClient http) => _http = http;

        public async Task StoreAsync(Guid chunkId, float[] vector, string tenantId)
        {
            await _http.PutAsJsonAsync("/collections/company_chunks/points", new
            {
                points = new[] { new { id = chunkId, vector, payload = new { tenantId } } }
            });
        }

        public async Task<List<Guid>> SearchAsync(float[] vector, string tenantId, int topK)
        {
            var res = await _http.PostAsJsonAsync("/collections/company_chunks/points/search", new
            {
                vector,
                limit = topK,
                filter = new
                {
                    must = new[] { new { key = "tenantId", match = new { value = tenantId } } }
                }
            });

            var json = await res.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("result")
                       .EnumerateArray()
                       .Select(x => Guid.Parse(x.GetProperty("id").GetString()!)).ToList();
        }
    }
}
