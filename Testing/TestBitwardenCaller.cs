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

        var output = caller.Login(loginData);

        Assert.AreEqual("not blank", output);
    }
}