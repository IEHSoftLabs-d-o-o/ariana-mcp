using Ariana_Mcp.Configuration;
using Ariana_Mcp.Okf;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var argv = new List<string>(args);

var builder = WebApplication.CreateBuilder(argv.ToArray());

builder.ConfigureAppSettings(argv.ToArray());
builder.ConfigureLoggingSettings();
builder.ConfigureArianaLab();
builder.Services.AddOkf(builder.Configuration);
builder.ConfigureSwagger();
builder.ConfigureMcp();
builder.Services.AddControllers();

var app = builder.Build();

app.UseConfiguredSwagger();
app.MapControllers();
app.UseConfiguredMcp();

await app.RunAsync();
