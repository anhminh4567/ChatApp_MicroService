using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

namespace ThreadLike.Common.Api
{
	public static class SetupProductionEnvironment
	{
		public static void ConfigureProductionSecret(this WebApplicationBuilder builder)
		{

			//string? clientId = builder.Configuration.GetSection("Azure:ManageIdentityClientId").Value;
			//ArgumentException.ThrowIfNullOrEmpty(clientId, "Client Id is required for Azure Key Vault access");

			string? keyVaultUrl = builder.Configuration.GetSection("Azure:KeyVault:BaseUrl").Value;
			ArgumentException.ThrowIfNullOrEmpty(keyVaultUrl, "Client Id is required for Azure Key Vault access");

			builder.Configuration.AddAzureKeyVault(
			   new Uri(keyVaultUrl),
			   new DefaultAzureCredential(
				   new DefaultAzureCredentialOptions()
				   {
					   
				   }),
			   new DelimiterResolveKeyVaultSecretManager());


		}
	}
	public class DelimiterResolveKeyVaultSecretManager() : KeyVaultSecretManager
	{
		private readonly string _delimiter = "-" ; // dash ' - ' is the only thing allowed in azure key vault

		public override string GetKey(KeyVaultSecret secret)
			=> secret.Name.Replace(_delimiter, ConfigurationPath.KeyDelimiter);
	}
}
