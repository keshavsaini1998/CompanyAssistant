using CompanyAssistant.Application.Interfaces;
using CompanyAssistant.Application.UseCases;
using CompanyAssistant.Infrastructure.Db;
using CompanyAssistant.Infrastructure.FileProcessing;
using CompanyAssistant.Infrastructure.Identity;
using CompanyAssistant.Infrastructure.Ollama;
using CompanyAssistant.Infrastructure.Vector;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Database
// --------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// =======================================================
// IDENTITY
// =======================================================
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// =======================================================
// AUTHENTICATION (JWT)
// =======================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
    };
});

builder.Services.AddAuthorization();

// --------------------
// Http Clients
// --------------------
builder.Services.AddHttpClient();

// --------------------
// Swagger 
// --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "CompanyAssistant API",
        Version = "v1"
    });

    // JWT support in Swagger
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });

    c.AddSecurityRequirement(new()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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

app.UseAuthentication();
app.UseAuthorization();

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
