using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Telemetry.Worker.State
{
    public class RedisAggregationStore
    {
        private readonly IDatabase _db;

        public RedisAggregationStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task AddAsync(
            string device, string metric, double value, DateTime timestamp)
        {
            var window = timestamp.ToString("yyyyMMddHHmm");
            var key = $"agg:{device}:{metric}:{window}";

            await _db.HashIncrementAsync(key, "sum", value);
            await _db.HashIncrementAsync(key, "count", 1);
        }
    }
}
