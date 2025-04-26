using MobyLabWebProgramming.Infrastructure.Extensions;
using Prometheus; // <-- adauga si importul acesta sus

var builder = WebApplication.CreateBuilder(args);

builder.AddCorsConfiguration()
    .AddRepository()
    .AddAuthorizationWithSwagger("MobyLab Web App")
    .AddServices()
    .UseLogger()
    .AddWorkers()
    .AddApi();

var app = builder.Build();

// --- adaugam aici middleware-urile pentru Prometheus ---
app.UseHttpMetrics();
app.MapMetrics();
// --------------------------------------------------------

app.ConfigureApplication();
app.Run();
