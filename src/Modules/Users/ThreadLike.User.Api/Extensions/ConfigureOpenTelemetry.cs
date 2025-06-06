﻿using Npgsql;
using Npgsql.PostgresTypes;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ThreadLike.Common.Infrastructure.OpenTelemetryExtend;

namespace ThreadLike.User.Api.Extensions
{
	public static class ConfigureOpenTelemetry
	{
		public static WebApplicationBuilder AddConfigureOpenTelemetry(this WebApplicationBuilder builder)
		{
			builder.Services.AddOpenTelemetry()
				.ConfigureResource(config => config.AddService(UserModuleMetaData.ServiceName + ".Module"))
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
					config.AddAspNetCoreInstrumentation(opt =>
					{
						opt.Filter = (context) => !context.Request.Path.StartsWithSegments("/metrics");
					});
					config.AddHttpClientInstrumentation();
					config.AddEntityFrameworkCoreInstrumentation(opt =>
					{
						opt.SetDbStatementForText = true;
					});
					config.AddNpgsql();
					config.AddRedisInstrumentation(opt =>
					{
						opt.EnrichActivityWithTimingEvents = true;
					});
					config.AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);
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
}
