using Serilog;

namespace ThreadLike.Chat.Api
{
	public static class AddLoggingConfiguration
	{
		public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
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
