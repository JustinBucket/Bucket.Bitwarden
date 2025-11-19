using System;
using System.Net;
using System.Threading;
using Bucket.Bitwarden;
using Bucket.Bitwarden.Auth;
using Testing.Helpers;

namespace Testing;

[TestClass]
public class TestBitwardenCaller
{
    [TestMethod]
    [ExpectedException(typeof(AggregateException))]
    public void TestUnlockWithNoAuth()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var success = caller.UnlockVault(secrets.MasterPassword).Result;
    }

    [TestMethod]
    [ExpectedException(typeof(AggregateException))]
    public void TestUnlockWithExpiredAuth()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var authResponse = caller.Authenticate(Helper.GenerateAuthData(secrets)).Result;
        Thread.Sleep(3_601_000);
        var success = caller.UnlockVault(secrets.MasterPassword).Result;
    }

    [TestMethod]
    public void TestAuthenticate()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var authData = new AuthData(secrets.ClientId, secrets.ClientSecret);

        var statusCode = caller.Authenticate(authData).Result;
    }
}