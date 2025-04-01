using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Infrastructure.Options
{
	public class JwtBearerConfigurationForSignalR : IConfigureNamedOptions<JwtBearerOptions>
	{
		public const string HubStartSegment = "/hubs";
		private readonly IConfiguration _configuration;

		public JwtBearerConfigurationForSignalR(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public void Configure(string? name, JwtBearerOptions options)
		{
			Configure(options);
		}

		public void Configure(JwtBearerOptions options)
		{
			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = (MessageReceivedContext context) =>
				{
					string? accessToken = context.Request.Query["access_token"];
					Microsoft.AspNetCore.Http.PathString path = context.HttpContext.Request.Path;
					if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments($"/{HubStartSegment}"))
					{
						context.Token = accessToken;
					}
					return Task.CompletedTask;
				}
			};
		}
	}
}
