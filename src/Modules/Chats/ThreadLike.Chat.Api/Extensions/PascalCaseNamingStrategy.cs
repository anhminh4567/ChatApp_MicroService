using Newtonsoft.Json.Serialization;

namespace ThreadLike.Chat.Api.Extensions
{
	public class PascalCaseNamingStrategy : NamingStrategy
	{
		public PascalCaseNamingStrategy()
		{
			ProcessDictionaryKeys = true;
			ProcessExtensionDataNames = true;
			OverrideSpecifiedNames = false;
		}
		protected override string ResolvePropertyName(string name)
		{
			if (string.IsNullOrEmpty(name))
				return name;
			return char.ToUpper(name[0]) + name.Substring(1);
		}
	}
}
