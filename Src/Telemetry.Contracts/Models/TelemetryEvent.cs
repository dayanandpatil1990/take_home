using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Contracts.Models
{
    public record TelemetryEvent(
     string DeviceId,
     string Interface,
     string Metric,
     double Value,
     DateTime Timestamp
 );
}

