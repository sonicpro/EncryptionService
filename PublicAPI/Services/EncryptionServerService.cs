using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PublicAPI.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    public class EncryptionServerService : IEncryptionServerService
    {
        private readonly IAuthorizationServerService authServerService;
        private readonly string encryptionApiBaseUrl;
        private readonly string encryptRouteTemplate;
        private readonly string decryptRouteTemplate;

        public EncryptionServerService(IAuthorizationServerService authorizationServerService, IOptions<ApiSettings> options)
        {
            authServerService = authorizationServerService;
            encryptionApiBaseUrl = options.Value.BaseEncryptionApiUrl;
            encryptRouteTemplate = options.Value.EncryptionRouteTemplate;
            decryptRouteTemplate = options.Value.DecryptionRouteTemplate;
        }

        public async Task<string> Decrypt(string encryptedData)
        {
            return await RequestApi(decryptRouteTemplate, encryptedData);
        }

        public async Task<string> Encrypt(string data)
        {
            return await RequestApi(encryptRouteTemplate, data);
        }

        #region Helper methods

        private async Task<string> RequestApi(string route, string payload)
        {
            var apiUrl = string.Join("/", encryptionApiBaseUrl, route);
            var accessToken = await authServerService.AcquireToken();
            var payloadJson = JsonConvert.SerializeObject(payload);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);
                using (HttpContent content = new StringContent(payloadJson))
                {
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    using (var responseMessage = await client.PostAsync(apiUrl, content))
                    {
                        return await responseMessage.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        #endregion
    }
}
