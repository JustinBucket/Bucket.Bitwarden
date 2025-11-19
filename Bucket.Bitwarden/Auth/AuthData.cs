using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.Bitwarden.Auth
{
    public class AuthData
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        // public ClientScope ClientScope { get; set; }

        public AuthData(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public Dictionary<string, string> Flatten()
        {
            // might need to add the other two for organizations
            return new Dictionary<string, string>()
            {
                {"client_id", ClientId},
                {"client_secret", ClientSecret},
                {"grant_type", "client_credentials"},
                {"deviceName", "fireFox"},
                {"deviceIdentifier", "0"},
                {"deviceType", "0"}
            };
        }
    }
}