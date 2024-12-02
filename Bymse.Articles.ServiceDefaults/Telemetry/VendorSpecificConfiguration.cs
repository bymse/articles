using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;
using Prometheus;

namespace Microsoft.Extensions.Hosting.Telemetry;

public static class VendorSpecificConfiguration
{
    public static void AddVendorSpecificTelemetry(this IHostApplicationBuilder builder)
    {
        ConfigurePrometheusMetrics(builder);
        ConfigureJaegerTracing(builder);
        ConfigureConsoleLogging(builder);
    }

    private static void ConfigurePrometheusMetrics(this IHostApplicationBuilder builder)
    {
        Metrics.SuppressDefaultMetrics(new SuppressDefaultMetricOptions
        {
            SuppressDebugMetrics = true,
            SuppressProcessMetrics = true,
            SuppressEventCounters = true,
        });

        builder.Services.AddMetricServer(e => { e.Port = 1234; });
    }

    private static void ConfigureJaegerTracing(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddOpenTelemetry()
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
            });
    }
    
    private static void ConfigureConsoleLogging(this IHostApplicationBuilder builder)
    {
        
    }
}