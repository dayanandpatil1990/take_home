using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Telemetry.Contracts.Models;

namespace Telemetry.Ingest.Services
{
    public class RedisStreamPublisher
    {
        private readonly IDatabase _db;
        private const string StreamName = "telemetry-stream";

        public RedisStreamPublisher(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task PublishAsync(TelemetryEvent evt)
        {
            await _db.StreamAddAsync(
                StreamName,
                new NameValueEntry[]
                {
                new("deviceId", evt.DeviceId),
                new("interface", evt.Interface),
                new("metric", evt.Metric),
                new("value", evt.Value),
                new("timestamp", evt.Timestamp.ToString("O"))
                });
        }
    }
}
