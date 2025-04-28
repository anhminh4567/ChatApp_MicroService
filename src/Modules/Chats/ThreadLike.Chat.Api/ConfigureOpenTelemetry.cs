using Microsoft.Extensions.Diagnostics.Metrics;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace ThreadLike.Chat.Api
{
	public static class ConfigureOpenTelemetry
	{
		public static WebApplicationBuilder AddConfigureOpenTelemetry(this WebApplicationBuilder builder)
		{

			builder.Services.AddOpenTelemetry()
			.ConfigureResource(config => config.AddService(ChatModuleMetaData.ServiceName + ".Module"))
			.WithMetrics((config) =>
			{
				config.AddAspNetCoreInstrumentation();
				config.AddHttpClientInstrumentation();
				config.AddRuntimeInstrumentation();
				config.AddProcessInstrumentation();
				config.AddPrometheusExporter(options =>
				{
					string? metricExporterEndpoint = builder.Configuration["Otlp:Metrics:ScapeEndpoint"];
					ArgumentException.ThrowIfNullOrEmpty(metricExporterEndpoint, "Metrics exporter endpoint is required");

					options.ScrapeResponseCacheDurationMilliseconds = 1000;
					options.ScrapeEndpointPath = "/metrics";//this is default
				});
			})
			.WithTracing((config) =>
			{
				
				config.AddAspNetCoreInstrumentation()
				.AddHttpClientInstrumentation()
				.AddSqlClientInstrumentation(opt =>
				{
					opt.SetDbStatementForText = true;
				})
				.AddEntityFrameworkCoreInstrumentation(opt => opt.SetDbStatementForText = true)
				.AddNpgsql()
				.AddRedisInstrumentation()
				.AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);
				config.AddProcessor(new ExternalServiceProcessor());

				config.AddOtlpExporter(options =>
				{
					string? traceExporterEndpoint = builder.Configuration["Otlp:Traces:PushEndpoint"];
					ArgumentException.ThrowIfNullOrEmpty(traceExporterEndpoint, "Trace exporter endpoint is required");

					options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
					options.Endpoint = new Uri(traceExporterEndpoint);
				});
			});



			return builder;
		}
	}

	public class ExternalServiceProcessor : BaseProcessor<Activity>
	{
		public override void OnEnd(Activity activity)
		{
			// For PostgreSQL
			if (activity.Tags.Any(tag => tag.Key == "db.system" && tag.Value == "postgresql"))
			{
				activity.SetTag("service.name", "PostgreSQL");
			}

			// For Redis
			if (activity.Tags.Any(tag => tag.Key == "db.system" && tag.Value == "redis"))
			{
				activity.SetTag("service.name", "Redis");
			}

			base.OnEnd(activity);
		}
	}
}
