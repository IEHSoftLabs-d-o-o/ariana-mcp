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
    try
    {
        var result = await authService.LoginAsync(
            request.User,
            request.Password,
            cancellationToken);

        if (result.LoginToken is null)
        {
            return Results.Text(
                result.ErrorMessage ?? "Login failed. Check user name and password.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Results.Ok(result);
    }
    catch (Exception)
    {
        return Results.Json(
            new LoginResponse { ErrorMessage = "Login failed because ArianaLab could not be reached." },
            statusCode: StatusCodes.Status500InternalServerError);
    }
})
.WithName("Login")
.WithTags("Auth")
.WithSummary("Login with ArianaLab user and password")
.Produces<LoginResponse>(StatusCodes.Status200OK)
.Produces<string>(StatusCodes.Status400BadRequest)
.Produces<LoginResponse>(StatusCodes.Status500InternalServerError);

app.MapGet("/system", async (
    HttpContext httpContext,
    SystemService systemService,
    CancellationToken cancellationToken) =>
{
    if (!ArianaLabBearerToken.TryParse(
            httpContext.Request.Headers.Authorization.ToString(),
            out var accessToken)
        || accessToken is null)
    {
        return Results.Json(
            new { errorMessage = "Missing or invalid Bearer token." },
            statusCode: StatusCodes.Status401Unauthorized);
    }

    if (accessToken.IsExpired)
    {
        return Results.Json(
            new { errorMessage = "The Bearer token has expired. Call POST /login again." },
            statusCode: StatusCodes.Status401Unauthorized);
    }

    var json = await systemService.GetSystemInfoAsync(cancellationToken);
    return Results.Content(json, "application/json");
})
.WithName("GetSystemInfo")
.WithTags("Auth")
.WithSummary("Calls ArianaLab currentUser with the Bearer token (same as get_system_info)")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status401Unauthorized);

app.MapOpenApi("/openapi.json");
app.MapMcp("/mcp");

await app.RunAsync();
