using System;
using System.Linq;
using Bucket.Bitwarden;
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

    [TestMethod]
    public void TestRetrieveSingleVaultItem()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var loginData = secrets.GenerateLoginData();

        caller.Login(loginData);

        var getParams = new GetParameters(secrets.TestVaultItemNameSingle);
        var entries = caller.RetrieveVaultItems(getParams);

        Assert.IsNotNull(entries);
        Assert.AreEqual(1, entries.Count);

        var entry = entries.First();
        Assert.AreEqual(secrets.TestVaultItemNameSingle, entry.Name);
        Assert.AreNotEqual(Guid.Empty, entry.Id);

        caller.Logout();
    }

    [TestMethod]
    public void TestRetrieveMultipleVaultItems()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var loginData = secrets.GenerateLoginData();

        caller.Login(loginData);

        var getParams = new GetParameters(secrets.TestVaultItemNameMultiple);
        var entries = caller.RetrieveVaultItems(getParams);

        Assert.IsNotNull(entries);
        Assert.IsTrue(entries.Count > 1);

        foreach (var entry in entries)
        {
            Assert.IsTrue(entry.Name.Contains(secrets.TestVaultItemNameMultiple));
            Assert.AreNotEqual(Guid.Empty, entry.Id);
        }

        caller.Logout();
    }

    [TestMethod]
    public void TestConsecutiveLoginAttempts()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var loginData = secrets.GenerateLoginData();

        caller.Login(loginData);
        caller.Login(loginData);
        var sessionKey = Environment.GetEnvironmentVariable("BW_SESSION");

        Assert.IsFalse(string.IsNullOrWhiteSpace(sessionKey));

        caller.Logout();
    }

    [TestMethod]
    public void TestCredentialPull()
    {
        var caller = new BitwardenCaller();
        var secrets = Helper.GenerateSecrets();
        var loginData = secrets.GenerateLoginData();

        caller.Login(loginData);

        var getParams = new GetParameters(secrets.TestVaultItemNameSingle);
        var entries = caller.RetrieveVaultItems(getParams);

        Assert.IsNotNull(entries);
        Assert.AreEqual(1, entries.Count);

        var entry = entries.First();
        Assert.AreEqual(secrets.TestVaultItemNameSingle, entry.Name);
        Assert.AreNotEqual(Guid.Empty, entry.Id);

        caller.Logout();
    }
}