using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bucket.Bitwarden.Auth;
using Bucket.Bitwarden.Get;
using Newtonsoft.Json;

namespace Bucket.Bitwarden
{
    public class BitwardenCaller
    {
        private readonly string LoginCommand = "bw.exe login {email} {password}";
        private readonly string LogoutCommand = "bw.exe logout";
        private readonly string SyncCommand = "bw.exe sync";
        private readonly string GetCommand = "bw.exe get {query}";
        private readonly string SessionKeyRegex = "(?<=BW_SESSION=\").+(?=\")";
        private readonly string GuidRegex = @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$";
        private readonly Process _cmd;

        public BitwardenCaller()
        {
            _cmd = Helpers.GenerateCmdProcess();
        }

        public void Login(LoginData data)
        {
            // call bw.exe, passing arguments
            var command = LoginCommand.Replace("{email}", data.Email).Replace("{password}", data.Password);

            // get output
            var commandOutput = ExecuteCommand(command).Trim();

            // want to check if we're already logged in
            if (commandOutput.Contains("You are already logged in"))
            {
                var sessionKey = Environment.GetEnvironmentVariable("BW_SESSION");
                if (string.IsNullOrWhiteSpace(sessionKey))
                {
                    // logout and relogin if we don't have the session key stored for some reason
                    Logout();
                    Login(data);
                }
            }
            else
            {
                // parse session key from output
                var sessionKey = Regex.Matches(commandOutput, SessionKeyRegex)[0].Value;
                Environment.SetEnvironmentVariable("BW_SESSION", sessionKey);
            }

            Sync();
        }

        public void Logout()
        {
            ExecuteCommand(LogoutCommand).Trim();
            Environment.SetEnvironmentVariable("BW_SESSION", null);
        }

        private void Sync()
        {
            ExecuteCommand(SyncCommand).Trim();
        }

        public ICollection<VaultItem> RetrieveVaultItems(GetParameters getParams)
        {
            var command = GetCommand.Replace("{query}", getParams.GenerateQueryString());
            var output = ExecuteCommand(command).Trim();

            // check if it's a json string
            // if is, parse into response
            // if not, parse again for each returned guid if they're there
            try
            {
                var vaultItem = JsonConvert.DeserializeObject<VaultItem>(output);
                
                return new List<VaultItem>() { vaultItem };
            }
            catch (Exception)
            {
                return RetrieveVaultItems(output);
            }
        }

        private IList<VaultItem> RetrieveVaultItems(string commandOutput)
        {
            var vaultItems = new List<VaultItem>();

            var ids = Regex.Matches(commandOutput, GuidRegex);
            
            foreach (var id in ids)
            {
                var getParams = new GetParameters(Guid.Parse(id.ToString()));
                var command = GetCommand.Replace("{query}", getParams.GenerateQueryString());

                var output = ExecuteCommand(command).Trim();
                var vaultItem = JsonConvert.DeserializeObject<VaultItem>(output);
                vaultItems.Add(vaultItem);
            }

            return vaultItems;
        }

        private string ExecuteCommand(string command)
        {
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

    }
}
