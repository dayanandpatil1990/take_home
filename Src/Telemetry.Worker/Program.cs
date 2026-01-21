
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Telemetry.Worker.Workers;
using Telemetry.Worker.State;
using Telemetry.Worker.Processing;
using Telemetry.Worker.Configuration;



Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect("localhost:6379"));

        services.AddSingleton<RedisAggregationStore>();
        services.AddSingleton<DynamicConfigProvider>();
        services.AddSingleton<AggregationService>();
        services.AddSingleton<AnomalyDetectionService>();

        services.AddHostedService<TelemetryStreamWorker>();
    })
    .Build()
    .Run();
