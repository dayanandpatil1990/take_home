using System.Net.Http.Json;


var client = new HttpClient();
var rnd = new Random();

while (true)
{
    var evt = new
    {
        deviceId = "router-01",
        @interface = "eth0",
        metric = "cpu_utilization",
        value = rnd.Next(50, 95),
        timestamp = DateTime.UtcNow
    };

    await client.PostAsJsonAsync(
        "http://localhost:5000/telemetry", evt);

    await Task.Delay(1000);
}
