using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bucket.Bitwarden;
using Bucket.Bitwarden.Auth;
using Newtonsoft.Json;

namespace Testing.Helpers;

public static class Helper
{
    public static Secrets GenerateSecrets()
    {
        var secretsText = File.ReadAllText(@"../../../Helpers/Secrets.json");
        var secrets = JsonConvert.DeserializeObject<Secrets>(secretsText);

        return secrets;
    }

    public static AuthData GenerateAuthData(this Secrets secrets)
    {
        return new AuthData(secrets.ClientId, secrets.ClientSecret);
    }

    public static LoginData GenerateLoginData(this Secrets secrets)
    {
        return new LoginData(secrets.Email, secrets.MasterPassword);
    }
}