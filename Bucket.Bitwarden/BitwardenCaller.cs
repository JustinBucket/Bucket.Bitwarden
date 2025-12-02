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
        // https://identity.bitwarden.com/connect/token
        // what if we made the whole thing based off the console?

        public BitwardenCaller()
        {
            _cmd = Helpers.GenerateCmdProcess();
        }

        public void Login(LoginData data)
        {
            // call bw.exe, passing arguments
            var command = LoginCommand.Replace("{email}", data.Email).Replace("{password}", data.Password);
            // var command = "dotnet --version";

            var commandOutput = ExecuteCommand(command).Trim();

            var sessionKey = Regex.Matches(commandOutput, SessionKeyRegex)[0].Value;

            Environment.SetEnvironmentVariable("BW_SESSION", sessionKey);
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

        public ICollection<VaultItem> RetrieveEntry(GetParameters getParams)
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
                return RetrieveEntries(output);
            }
        }

        private IList<VaultItem> RetrieveEntries(string commandOutput)
        {
            var vaultItems = new List<VaultItem>();

            var ids = Regex.Matches(commandOutput, @"[0-9a-fA-F]{8}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{12}");
            
            foreach (var id in ids)
            {
                var getParams = new GetParameters(Guid.Parse(id.ToString()));
                var command = GetCommand.Replace("{query}", getParams.GenerateQueryString());

                var output = ExecuteCommand(command).Trim();
                var vaultItem = JsonConvert.DeserializeObject<VaultItem>(output);
                vaultItems.Add(vaultItem);
            }

            // check if it's a json string
            // if is, parse into response
            // if not, parse again for each returned guid if they're there

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
