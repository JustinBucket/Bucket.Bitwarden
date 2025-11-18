using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
}