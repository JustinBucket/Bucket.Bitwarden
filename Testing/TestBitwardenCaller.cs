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
    public void TestLogin()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var loginData = secrets.GenerateLoginData();

        caller.Login(loginData);
        var sessionKey = Environment.GetEnvironmentVariable("BW_SESSION");

        Assert.IsFalse(string.IsNullOrWhiteSpace(sessionKey));

        caller.Logout();
    }

    [TestMethod]
    public void TestLogout()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var loginData = secrets.GenerateLoginData();

        caller.Login(loginData);
        caller.Logout();
        var sessionKey = Environment.GetEnvironmentVariable("BW_SESSION");

        Assert.IsTrue(string.IsNullOrWhiteSpace(sessionKey));
    }
}