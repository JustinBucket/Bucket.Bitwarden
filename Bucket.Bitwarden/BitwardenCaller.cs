using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly string LoginCommand = "bw.exe login {email} {password}";
        private readonly Process _cmd;
        private string _sessionKey;
        // https://identity.bitwarden.com/connect/token
        // what if we made the whole thing based off the console?

        public BitwardenCaller()
        {
            _cmd = Helpers.GenerateCmdProcess();
        }

        // TODO: add handling for already logged in error
        public string Login(LoginData data)
        {
            // call bw.exe, passing arguments
            var command = LoginCommand.Replace("{email}", data.Email).Replace("{password}", data.Password);
            // var command = "dotnet --version";
            
            return ExecuteCommand(command);
        }

        private string ExecuteCommand(string command)
        {
            // var process = new Process();
            // process.StartInfo.FileName = "cmd.exe";
            // process.StartInfo.Arguments = $"/C {command}";
            // process.StartInfo.UseShellExecute = false;
            // process.StartInfo.CreateNoWindow = true;
            // process.StartInfo.RedirectStandardInput = true;
            // process.StartInfo.RedirectStandardOutput = true;
            // process.Start();

            // var output = process.StandardOutput.ReadToEnd();

            // process.Close();

            _cmd.StartInfo.Arguments = $"/C {command}";
            _cmd.Start();
            var output = _cmd.StandardOutput.ReadToEnd();
            if (string.IsNullOrWhiteSpace(output))
            {
                output = _cmd.StandardError.ReadToEnd();
            }
            _cmd.StandardInput.Close();

            return output;
        }

        // public async Task<HttpStatusCode> UnlockVault(string masterPassword)
        // {
        //     // this is going to need to use their cli apparently?
        //     // we have to log into the cli first, then we can maybe serve?
        //     // asks for OTP in email
        //     // what a mess
        //     // check if we've got an expiry date, meaning haven't performed authentication
        //     if (_authExpiryDate.Equals(DateTime.MinValue))
        //     {
        //         throw new AuthException("Authentication not performed");
        //     }

        //     // check if passed authentication lifetime
        //     if (DateTime.Now > _authExpiryDate)
        //     {
        //         throw new AuthException("Authentication expired");
                
        //     }

        //     var passwordDto = new UnlockRequestDto(masterPassword);
        //     var content = new StringContent(JsonConvert.SerializeObject(passwordDto), Encoding.UTF8, "application/json");
        //     var response = await _client.PostAsync(@"unlock", content);

        //     if (response.IsSuccessStatusCode)
        //     {
        //         var responseContent = await response.Content.ReadAsStringAsync();
        //         var responseData = JsonConvert.DeserializeObject<UnlockData>(responseContent);
        //         _sessionKey = responseData.Raw;
        //     }

        //     return response.StatusCode;
        // }

    }
}
