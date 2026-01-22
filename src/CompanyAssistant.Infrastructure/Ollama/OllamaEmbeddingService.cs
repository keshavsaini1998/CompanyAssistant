using CompanyAssistant.Application.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace CompanyAssistant.Infrastructure.Ollama
{
    public class OllamaEmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _http;
        public OllamaEmbeddingService(HttpClient http) => _http = http;

        public async Task<float[]> EmbedAsync(string text)
        {
            var res = await _http.PostAsJsonAsync("/api/embeddings", new
            {
                model = "nomic-embed-text",
                prompt = text
            });

            var json = await res.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("embedding").EnumerateArray().Select(x => x.GetSingle()).ToArray();
        }
    }
}
