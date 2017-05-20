using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Pollux.Authentication;
using Axis.Pollux.RBAC.Auth;
using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule;
using Axis.Pollux.RBAC.OAModule;
using System.Configuration;
using System.Data.Entity;

namespace Axis.Pollux.Tests
{
    [TestClass]
    public class UnitTest1
    {
        readonly ContextConfiguration<EuropaContext> Config = new ContextConfiguration<EuropaContext>()
                   .WithConnection(ConfigurationManager.ConnectionStrings["EuropaContext"].ConnectionString)
                   .WithInitializer(new DropCreateDatabaseIfModelChanges<EuropaContext>())
                   .UsingModule(new IdentityAccessModuleConfig())
                   .UsingModule(new RBACAccessModuleConfig())
                   .UsingModule(new Authentication.OAModule.AuthenticationAccessModuleConfig());


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

        [TestMethod]
        public void ResourceSelection()
        {
            var r = new ResourceSelector(":system/settings/*");
            Assert.IsTrue(r.Match("/:system/settings/@get"));
        }

        [TestMethod]
        public void EuropaSpeedTest()
        {
            var start = DateTime.Now;
            var europa = new EuropaContext(Config);
            Console.WriteLine("Context created in: " + (DateTime.Now - start));

            start = DateTime.Now;
            europa = new EuropaContext(Config);
            Console.WriteLine("Context created in: " + (DateTime.Now - start));

            start = DateTime.Now;
            europa = new EuropaContext(Config);
            Console.WriteLine("Context created in: " + (DateTime.Now - start));

            start = DateTime.Now;
            europa = new EuropaContext(Config);
            Console.WriteLine("Context created in: " + (DateTime.Now - start));

            start = DateTime.Now;
            europa = new EuropaContext(Config);
            Console.WriteLine("Context created in: " + (DateTime.Now - start));

            start = DateTime.Now;
            europa = new EuropaContext(Config);
            Console.WriteLine("Context created in: " + (DateTime.Now - start));

            start = DateTime.Now;
            europa = new EuropaContext(Config);
            Console.WriteLine("Context created in: " + (DateTime.Now - start));
        }
    }
}
