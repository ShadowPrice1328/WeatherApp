using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using ServiceContracts;
using ServiceContracts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ApiKeyValidation : IApiKeyValidation
    {
        private readonly IConfiguration _configuration;
        private readonly SecretClient _secretClient;

        public ApiKeyValidation(IConfiguration configuration, SecretClient secretClient)
        {
            _configuration = configuration;
            _secretClient = secretClient;
        }

        public bool IsValidApiKey(string userApiKey)
        {
            if (string.IsNullOrWhiteSpace(userApiKey))
                return false;

            KeyVaultSecret apiKeySecret = _secretClient.GetSecret(Constants.ApiKeyName);
            string? apiKey = apiKeySecret.Value;

            if (apiKey == null || apiKey != userApiKey)
                return false;

            return true;
        }
    }
}
