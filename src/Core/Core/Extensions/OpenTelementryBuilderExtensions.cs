using Core;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


namespace Core.Extensions
{
    public static class OpenTelemetryBuilderExtensions
    {
        public static OpenTelemetryBuilder AddLogging(this OpenTelemetryBuilder otlpBuilder, WebApplicationBuilder builder)
        {
            var serviceName = DiagnosticsConfig.GetServiceName(builder.Environment.ApplicationName);

            otlpBuilder
                .WithLogging(options =>
                {
                    options
                        .ConfigureResource(resourceBuilder =>
                        {
                            resourceBuilder.AddService(serviceName);
                        })
                        .AddOtlpExporter(opt =>
                        {
                            opt.Protocol = OtlpExportProtocol.Grpc;
                            opt.Endpoint = new Uri(builder.Configuration["OpenTelemetry:Exporter:Otlp:Endpoint"]);
                        });
                });

            return otlpBuilder;
        }

        public static OpenTelemetryBuilder AddTracing(this OpenTelemetryBuilder otlpBuilder, WebApplicationBuilder builder)
        {
            var serviceName = DiagnosticsConfig.GetServiceName(builder.Environment.ApplicationName);

            otlpBuilder
                .WithTracing(options =>
                {
                    options
                        .AddSource(DiagnosticsConfig.ActivitySource.Name)
                        .ConfigureResource(resourceBuilder =>
                        {
                            resourceBuilder.AddService(serviceName);
                        })
                        .SetErrorStatusOnException()
                        .SetSampler(new AlwaysOnSampler()) // For dev. purposes only.
                        .AddHttpClientInstrumentation(instrumentationOptions =>
                        {
                            instrumentationOptions.RecordException = true;
                        })
                        .AddAspNetCoreInstrumentation(instrumentationOptions =>
                        {
                            instrumentationOptions.RecordException = true;
                        })
                        .AddSqlClientInstrumentation(instrumentationOptions =>
                        {
                            instrumentationOptions.RecordException = true;
                            instrumentationOptions.SetDbStatementForText = true;
                        })
                        .AddEntityFrameworkCoreInstrumentation(instrumentationOptions =>
                        {
                            instrumentationOptions.SetDbStatementForText = true;
                        })
                        .AddOtlpExporter(opt =>
                        {
                            opt.Protocol = OtlpExportProtocol.Grpc;
                            opt.Endpoint = new Uri(builder.Configuration["OpenTelemetry:Exporter:Otlp:Endpoint"]);
                        });
                });

            return otlpBuilder;
        }

        public static OpenTelemetryBuilder AddMetrics(this OpenTelemetryBuilder otlpBuilder,
            WebApplicationBuilder builder)
        {
            var serviceName = DiagnosticsConfig.GetServiceName(builder.Environment.ApplicationName);

            otlpBuilder
                .WithMetrics(options =>
                {
                    options
                        .ConfigureResource(resourceBuilder =>
                        {
                            resourceBuilder.AddService(serviceName);
                        })
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddEventCountersInstrumentation()
                        .AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel", "System.Net.Http")
                        .AddCustomMeter(builder.Environment.ApplicationName)
                        //.AddPrometheusExporter() // exporting metrics to prometheus via prometheus-net exporter.
                        .AddOtlpExporter(opt =>
                        {
                            opt.Protocol = OtlpExportProtocol.Grpc;
                            opt.Endpoint = new Uri(builder.Configuration["OpenTelemetry:Exporter:Otlp:Endpoint"]);
                        });
                });

            return otlpBuilder;
        }

        public static OpenTelemetryBuilder AddObservability(this OpenTelemetryBuilder otlpBuilder,
            WebApplicationBuilder builder)
        {
            return otlpBuilder
                .AddLogging(builder)
                .AddTracing(builder)
                .AddMetrics(builder);
        }
    }
}
