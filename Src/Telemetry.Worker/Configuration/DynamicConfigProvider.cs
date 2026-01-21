using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Telemetry.Worker.Configuration
{
    public class DynamicConfigProvider
    {
        private readonly IDatabase _db;
        private Dictionary<string, double> _cache = new();

        public DynamicConfigProvider(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
            Refresh();
        }

        public double GetThreshold(string metric)
        {
            Refresh();
            return _cache.TryGetValue(metric, out var t) ? t : double.MaxValue;
        }

        private void Refresh()
        {
            var entries = _db.HashGetAll("anomaly-config");
            _cache = entries.ToDictionary(
                e => e.Name.ToString(),
                e => (double)e.Value);
        }
    }
}
