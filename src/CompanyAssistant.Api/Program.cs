using CompanyAssistant.Application.Interfaces;
using CompanyAssistant.Application.UseCases;
using CompanyAssistant.Infrastructure.Db;
using CompanyAssistant.Infrastructure.FileProcessing;
using CompanyAssistant.Infrastructure.Ollama;
using CompanyAssistant.Infrastructure.Vector;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Database
// --------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// --------------------
// Http Clients
// --------------------
builder.Services.AddHttpClient();

// --------------------
// Swagger 
// --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() { Title = "CompanyAssistant API", Version = "v1" }); });

// --------------------
// Infrastructure → Application interface bindings
// --------------------
builder.Services.AddScoped<IEmbeddingService, OllamaEmbeddingService>();
builder.Services.AddScoped<IChatService, OllamaChatService>();
builder.Services.AddScoped<IVectorStore, QdrantVectorStore>();
builder.Services.AddScoped<IDocumentRepository, EfDocumentRepository>();

// --------------------
// Application Use Cases
// --------------------
builder.Services.AddScoped<UploadDocumentHandler>();
builder.Services.AddScoped<AskQuestionHandler>();

var app = builder.Build();

// --------------------
// Middleware
// --------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CompanyAssistant API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// --------------------
// Endpoints
// --------------------
app.MapPost("/api/documents/upload", async (
    IFormFile file,
    string tenantId,
    string role,
    UploadDocumentHandler handler) =>
{
    var content = FileTextExtractor.Extract(file);

    await handler.HandleAsync(new UploadDocumentCommand(
        tenantId,
        role,
        file.FileName,
        content));

    return Results.Ok();
});

app.MapPost("/api/ask", async (
    AskQuestionQuery query,
    AskQuestionHandler handler) =>
{
    var answer = await handler.HandleAsync(query);
    return Results.Ok(answer);
});

app.Run();
