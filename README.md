# NET8-Monitoring-w-OpenTelemetry-Loki-Tempo-Prometheus-and-Grafana
## This repo is updated version of existing 'CentralizedLoggingAndTracingSolution' repo, where I used action filters and middlewares to get the job done. so multi-tenant, multi-service part is same as before, but following is changed:
- Introduced **OpenTelemetry** running at port **4317**.
- Application will generate and throw logs, traces and metrics to openTelemetry endpoint at port 4317.
- Then from port 4317, **Promtail** scrape logs to **Loki**, traces will be pushed to **Tempo**, and metrics to **Prometheus** via opentelemetry config.
- Later you can visualized this telemetry data to **Grafana** at port **3000**.

https://docs.google.com/presentation/d/1FmmhMc3zcuCK8zhdT0okRChn5mPd1Z8zoYXeKIaSTo8/edit?usp=sharing
