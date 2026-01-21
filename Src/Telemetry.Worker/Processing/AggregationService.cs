using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Telemetry.Worker.State;

namespace Telemetry.Worker.Processing
{
    public class AggregationService
    {
        private readonly RedisAggregationStore _store;
        private readonly AnomalyDetectionService _anomaly;

        public AggregationService(
            RedisAggregationStore store,
            AnomalyDetectionService anomaly)
        {
            _store = store;
            _anomaly = anomaly;
        }

        public async Task ProcessAsync(StreamEntry entry)
        {
            var device = entry["deviceId"];
            var metric = entry["metric"];
            var value = double.Parse(entry["value"]);
            var timestamp = DateTime.Parse(entry["timestamp"]);

            await _store.AddAsync(device, metric, value, timestamp);
            _anomaly.Check(device, metric, value);
        }
    }
}
