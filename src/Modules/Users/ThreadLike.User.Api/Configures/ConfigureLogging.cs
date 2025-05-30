﻿using Serilog;

namespace ThreadLike.User.Api.Configures
{
	public static class ConfigureLogging
	{
		public static IServiceCollection AddAndConfigureLogging(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSerilog((services, config) =>
			{
				config.ReadFrom.Configuration(configuration);
				config.WriteTo.OpenTelemetry((x) =>
				{

					string? endpoint = configuration["Otlp:Logs:Endpoint"];
					ArgumentException.ThrowIfNullOrEmpty(endpoint);
					endpoint = $"{endpoint}/ingest/otlp/v1/logs";
					x.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.HttpProtobuf;
					x.Endpoint = endpoint;
					x.Headers = new Dictionary<string, string>
					{
						{ "X-Seq-ApiKey", "9UgbPK8d9J3SOiYGHgyx" }
					};

				});
			}, false, false);
			return services;
		}
	}
}
