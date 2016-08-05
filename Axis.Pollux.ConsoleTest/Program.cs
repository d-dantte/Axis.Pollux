using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule;
using System;
using System.Linq;

namespace Axis.Pollux.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ContextConfiguration()
                .WithConnection("EuropaContext")
                .WithInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<EuropaContext>())
                .UsingModule(new IdentityAccessModuleConfig())
                .UsingModule(new Authentication.OAModule.AuthenticationAccessModuleConfig());

            using (var cxt = new EuropaContext(config))
            {
                var x = cxt.Store<Authentication.Credential>().Query.FirstOrDefault();
            }

            Console.ReadKey();
        }
    }
}
