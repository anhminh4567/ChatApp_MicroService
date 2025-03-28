using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Infrastructure.Authentication
{
	public class AuthenticationOptions
	{
		public const string SectionName = "Authentication";
		public string ClientId { get; set; }
		public string MetadataAddress { get; set; }
		public string Authority { get; set; }
		public string TokenEndpoint { get; set; }
		public string UserInfoEndpoint { get; set; }
		public string RedirectUri { get; set; }
		public string ClientSecret { get; set; }
		public string BaseUrl { get; set; }	
	}
}
