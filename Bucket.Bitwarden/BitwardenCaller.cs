using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bucket.Bitwarden.Auth;
using Newtonsoft.Json;

namespace Bucket.Bitwarden
{
    public class BitwardenCaller
    {
        private readonly HttpClient _client;
        private readonly AuthCaller _authCaller;
        private DateTime _authExpiryDate;
        private string _sessionKey;
        // https://identity.bitwarden.com/connect/token

        public BitwardenCaller(string endpoint = @"https://api.bitwarden.com/public/")
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(endpoint)
            };
            _authCaller = new AuthCaller();
        }

        public async Task<bool> Authenticate(AuthData data)
        {
            var authResponse = await _authCaller.Authenticate(data);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.AccessToken);
            _authExpiryDate = DateTime.Now.AddSeconds(authResponse.ExpiresIn);

            return true;
        }

        public async Task<HttpStatusCode> UnlockVault(string masterPassword)
        {
            // this is going to need to use their cli apparently?
            // we have to log into the cli first, then we can maybe serve?
            // asks for OTP in email
            // what a mess
            // check if we've got an expiry date, meaning haven't performed authentication
            if (_authExpiryDate.Equals(DateTime.MinValue))
            {
                throw new AuthException("Authentication not performed");
            }

            // check if passed authentication lifetime
            if (DateTime.Now > _authExpiryDate)
            {
                throw new AuthException("Authentication expired");
                
            }

            var passwordDto = new UnlockRequestDto(masterPassword);
            var content = new StringContent(JsonConvert.SerializeObject(passwordDto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(@"unlock", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<UnlockData>(responseContent);
                _sessionKey = responseData.Raw;
            }

            return response.StatusCode;
        }

    }
}
