using System.Reflection;
using Ariana_Mcp.Auth;
using Ariana_Mcp.Configuration;
using Ariana_Mcp.integrations.Models;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Integrations.AraianLab;
using Ariana_Mcp.Mcp;
using Ariana_Mcp.Okf;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using ModelContextProtocol.Server;

var argv = new List<string>(args);

var builder = WebApplication.CreateBuilder(argv.ToArray());

builder.ConfigureAppSettings(argv.ToArray());
builder.ConfigureLoggingSettings();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IArianaLabRequestAuth, HttpContextArianaLabRequestAuth>();
builder.Services.AddAraianLabHttpClient(builder.Configuration);
builder.Services.AddOkf(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "Token",
        In = ParameterLocation.Header,
        Description = "Paste loginToken.token from POST /login. Swagger adds the Bearer prefix.",
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
    });
});

builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInstructions = ServerInstructions.Text;
    })
    .WithHttpTransport(o => o.Stateless = true)
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly();

var app = builder.Build();

// Tool types are activated lazily per call, so an unregistered dependency would only
// surface as a failed tool invocation. Activate each one once to fail fast at startup.
using (var scope = app.Services.CreateScope())
{
    foreach (var toolType in Assembly.GetExecutingAssembly().GetTypes()
        .Where(type => !type.IsAbstract && type.GetCustomAttribute<McpServerToolTypeAttribute>() is not null))
    {
        ActivatorUtilities.CreateInstance(scope.ServiceProvider, toolType);
    }
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ariana MCP");
    options.RoutePrefix = "swagger";
});

app.MapGet("/", () =>
{
    var asm = Assembly.GetExecutingAssembly();
    var name = asm.GetName().Name ?? "Ariana-Mcp";
    var version =
        asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? asm.GetName().Version?.ToString()
        ?? "unknown";

    return Results.Ok(new { name, version });
});

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/login", async (
    LoginRequest request,
    AuthService authService,
    CancellationToken cancellationToken) =>
{
    var result = await authService.LoginAsync(
        request.User,
        request.Password,
        cancellationToken);
    return Results.Ok(result);
})
.WithName("Login")
.WithTags("Auth")
.WithSummary("Login with ArianaLab user and password")
.Produces<LoginResponse>();

app.MapGet("/system", async (SystemService systemService, CancellationToken cancellationToken) =>
{
    var json = await systemService.GetSystemInfoAsync(cancellationToken);
    return Results.Content(json, "application/json");
})
.WithName("GetSystemInfo")
.WithTags("Auth")
.WithSummary("Calls ArianaLab currentUser with the Bearer token (same as get_system_info)");

app.MapOpenApi("/openapi.json");
app.MapMcp("/mcp");

await app.RunAsync();
