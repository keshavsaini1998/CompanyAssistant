using CompanyAssistant.Application.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace CompanyAssistant.Infrastructure.Ollama
{
    public class OllamaChatService : IChatService
    {
        private readonly HttpClient _http;
        public OllamaChatService(HttpClient http) => _http = http;

        public async Task<string> AskAsync(string prompt)
        {
            var res = await _http.PostAsJsonAsync("http://localhost:11434/api/generate", new
            {
                model = "llama3.2:3b",
                prompt,
                stream = false
            });

            var json = await res.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("response").GetString()!;
        }
    }
}
