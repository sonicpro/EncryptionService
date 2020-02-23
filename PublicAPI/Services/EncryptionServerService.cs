using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PublicAPI.Interfaces;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    public class EncryptionServerService : IEncryptionServerService
    {
        private const string EncodingHeader = "br";
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
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(EncodingHeader));
                using (HttpContent content = new StringContent(payloadJson))
                {
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    using (var responseMessage = await client.PostAsync(apiUrl, content))
                    {
                        return await ProcessContent(responseMessage.Content);
                    }
                }
            }
        }

        private async Task<string> ProcessContent(HttpContent content)
        {
            string contentPayload = null;

            if (content.Headers.ContentEncoding.Contains(EncodingHeader))
            {
                using (var input = await content.ReadAsStreamAsync())
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var dc = new BrotliStream(input, CompressionMode.Decompress))
                        {
                            dc.CopyTo(ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            using (var sr = new StreamReader(ms, Encoding.UTF8))
                            {
                                contentPayload = await sr.ReadToEndAsync();
                            }
                        }
                    }
                }
                
            }
            else
            {
                contentPayload = await content.ReadAsStringAsync();
            }
            return contentPayload;
        }

        #endregion
    }
}
