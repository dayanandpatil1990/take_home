1. Telemetry.DeviceSimulator
Purpose:
Simulates real network devices sending telemetry data.

Responsibilities:

Periodically emits telemetry events

Mimics realistic metric ranges

Helps validate system end-to-end

2. Telemetry.Ingest (HTTP API)

Purpose:
Acts as the ingestion edge for telemetry events.

Responsibilities:

Accepts telemetry via REST (POST /telemetry)

Performs minimal validation

Publishes events to Redis Streams

3. Redis Streams (Event Backbone)

Purpose:
Decouples ingestion from processing.
4. Telemetry.Worker (Distributed Processor)

Purpose:
Consumes telemetry events and performs processing.

Responsibilities:

Reads events from Redis Stream consumer group

Performs aggregation

Runs anomaly detection

Acknowledges processed events

Scalability Model:

Multiple workers can run concurrently

Each worker gets a subset of messages

Horizontal scaling without code changes

5. AggregationService

Purpose:
Performs time-windowed aggregation.

Aggregation Strategy:

1-minute time buckets

Per device + per metric

Stores sum and count

Enables later calculation of averages
6. AnomalyDetectionService

Purpose:
Detects abnormal metric values in real time.

Current Logic (Simple by Design):

Threshold-based detection

Metric-specific limits
7. DynamicConfigProvider

Purpose:
Allows runtime configuration changes without redeploying services.

How it works:

Reads anomaly thresholds from Redis

Caches values in memory

Refreshes dynamically
