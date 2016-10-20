using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Pollux.Authentication;

namespace Axis.Pollux.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cred = new Credential
            {
                ExpiresIn = null,
                Metadata = CredentialMetadata.Password,
                Status = CredentialStatus.Active
            };

            Console.WriteLine(cred);
        }
    }
}
