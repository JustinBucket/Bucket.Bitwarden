using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.Helpers;

namespace Testing
{
    [TestClass]
    public class TestHelpers
    {
        [TestMethod]
        public void TestSecretsCreation()
        {
            var secrets = Helper.GenerateSecrets();

            Assert.IsFalse(string.IsNullOrWhiteSpace(secrets.MasterPassword));
        }
    }
}