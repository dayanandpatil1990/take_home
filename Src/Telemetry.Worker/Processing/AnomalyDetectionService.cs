using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Worker.Configuration;

namespace Telemetry.Worker.Processing
{
    public class AnomalyDetectionService
    {
        private readonly DynamicConfigProvider _config;

        public AnomalyDetectionService(DynamicConfigProvider config)
        {
            _config = config;
        }

        public void Check(string device, string metric, double value)
        {
            var threshold = _config.GetThreshold(metric);
            if (value > threshold)
            {
                Console.WriteLine(
                    $" ANOMALY: {device} {metric} = {value}");
            }
        }
    }
}
