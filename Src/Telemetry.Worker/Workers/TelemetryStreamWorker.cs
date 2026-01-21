using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Telemetry.Worker.Processing;

namespace Telemetry.Worker.Workers
{
    public class TelemetryStreamWorker : BackgroundService
    {
        private readonly IDatabase _db;
        private readonly AggregationService _aggregation;
        private const string Stream = "telemetry-stream";
        private const string Group = "telemetry-workers";
        private readonly string _consumer = Guid.NewGuid().ToString();

        public TelemetryStreamWorker(
            IConnectionMultiplexer redis,
            AggregationService aggregation)
        {
            _db = redis.GetDatabase();
            _aggregation = aggregation;

            try
            {
                _db.StreamCreateConsumerGroup(Stream, Group, "$", true);
            }
            catch { }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var entries = await _db.StreamReadGroupAsync(
                    Stream, Group, _consumer, ">", 10);

                foreach (var entry in entries)
                {
                    await _aggregation.ProcessAsync(entry);
                    await _db.StreamAcknowledgeAsync(Stream, Group, entry.Id);
                }
            }
        }
    }
}
