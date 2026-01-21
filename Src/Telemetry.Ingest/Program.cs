
using StackExchange.Redis;
using Telemetry.Contracts.Models;
using Telemetry.Ingest.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379"));

builder.Services.AddSingleton<RedisStreamPublisher>();

var app = builder.Build();

app.MapPost("/telemetry", async (
    TelemetryEvent evt,
    RedisStreamPublisher publisher) =>
{
    await publisher.PublishAsync(evt);
    return Results.Accepted();
});

app.Run();
