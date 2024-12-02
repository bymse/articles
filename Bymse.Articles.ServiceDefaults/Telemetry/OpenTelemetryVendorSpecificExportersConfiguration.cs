using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.Hosting.Telemetry;

public static class OpenTelemetryVendorSpecificExportersConfiguration
{
    //https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Exporter.Prometheus.AspNetCore
    
    public static void AddOpenTelemetryVendorSpecificExporters(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });
        
        builder.Services
            .AddOpenTelemetry()
            .WithLogging(logging =>
            {
                logging
                    .AddConsoleExporter();
            })
            .WithMetrics(e =>
            {
                e
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter("MassTransit")
                    .AddMeter("Collector")
                    .AddPrometheusExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddSource("MassTransit")
                    .AddSource("Bymse.Articles.Collector")
                    .AddSource("MailKit.Net.ImapClient")
                    .AddOtlpExporter(e =>
                    {
                        e.Protocol = OtlpExportProtocol.HttpProtobuf;
                        e.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_TRACES_ENDPOINT")!);
                    });
            });;
    }
}