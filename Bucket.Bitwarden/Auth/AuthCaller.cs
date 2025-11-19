using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bucket.Bitwarden.Auth
{
    internal class AuthCaller
    {
        internal HttpClient _client;
        internal AuthCaller()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(@"https://identity.bitwarden.com/connect/")
            };
        }
        internal async Task<AuthResponse> Authenticate(AuthData data)
        {

            var req = new HttpRequestMessage(HttpMethod.Post, "token") { Content = new FormUrlEncodedContent(data.Flatten()) };
            var response = await _client.SendAsync(req);
            var responseMessage = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Authentication failed: '{responseMessage}'");
            }

            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseMessage);

            return authResponse;
        }
    }
}