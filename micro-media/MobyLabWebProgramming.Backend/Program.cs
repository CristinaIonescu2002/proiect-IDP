using Prometheus;
using MobyLabWebProgramming.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddCorsConfiguration()
    .AddRepository()
    .AddAuthorizationWithSwagger("MobyLab Web App")
    .AddServices()
    .UseLogger()
    .AddWorkers()
    .AddApi();

// aici înregistrezi metricile HTTP
builder.Services.AddHttpMetrics();

var app = builder.Build();

app.UseRouting();

// middleware pentru metrici
app.UseHttpMetrics();
app.MapMetrics();

app.ConfigureApplication();
app.Run();
