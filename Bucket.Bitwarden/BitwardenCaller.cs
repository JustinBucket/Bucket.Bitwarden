using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bucket.Bitwarden
{
    public class BitwardenCaller
    {
        private readonly HttpClient _client;
        private string sessionKey;

        public BitwardenCaller(string endpoint = @"https://api.bitwarden.com/")
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(endpoint)
            };
        }

        // TODO: need to authenticate first
        public async Task<HttpStatusCode> UnlockVault(string masterPassword)
        {
            var passwordDto = new UnlockRequestDto(masterPassword);
            var content = new StringContent(JsonConvert.SerializeObject(passwordDto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(@"unlock", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<UnlockData>(responseContent);
                sessionKey = responseData.Raw;
            }

            return response.StatusCode;
        }

    }
}
