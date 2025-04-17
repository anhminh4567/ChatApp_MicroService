using Newtonsoft.Json.Serialization;

namespace ThreadLike.Chat.Api.Extensions
{
	public class PascalCaseContractResolver : DefaultContractResolver
	{
		public PascalCaseContractResolver()
		{
			NamingStrategy = new PascalCaseNamingStrategy();

		}

		protected override string ResolvePropertyName(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
				return propertyName;

			return char.ToUpper(propertyName[0]) + propertyName.Substring(1);
		}

		
	}
}
