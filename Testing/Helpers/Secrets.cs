using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testing.Helpers;

public class Secrets
{
    public string MasterPassword { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Email { get; set; }
    public string TestVaultItemNameSingle { get; set; }
    public string TestVaultItemNameMultiple { get; set; }
}