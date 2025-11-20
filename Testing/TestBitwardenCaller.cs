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
    // need to run the test for an hour
    // I can modify the expiry so that it sets it to a shorter time frame ahead
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

    [TestMethod]
    public void TestUnlock()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var authData = Helper.GenerateAuthData(secrets);

        var _ = caller.Authenticate(authData).Result;
        var unlockStatusCode = caller.UnlockVault(secrets.MasterPassword).Result;

        Assert.AreEqual(HttpStatusCode.OK, unlockStatusCode);
    }
}