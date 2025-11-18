using System.Net;
using Bucket.Bitwarden;
using Testing.Helpers;

namespace Testing;

[TestClass]
public class TestBitwardenCaller
{
    [TestMethod]
    public void TestUnlock()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var success = caller.UnlockVault(secrets.MasterPassword).Result;

    }
}