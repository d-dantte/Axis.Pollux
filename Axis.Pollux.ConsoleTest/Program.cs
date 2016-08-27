using Axis.Jupiter.Europa;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.OAModule;
using Axis.Pollux.Identity.Principal;
using System;
using System.Data.Entity;
using System.Linq;

namespace Axis.Pollux.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ContextConfiguration<EuropaContext>()
                .WithConnection("EuropaContext")
                .WithInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<EuropaContext>())
                .UsingModule(new IdentityAccessModuleConfig())
                //.UsingModule(new XyzModule())
                .UsingModule(new Authentication.OAModule.AuthenticationAccessModuleConfig());

            using (var cxt = new EuropaContext(config))
            {
                var x = cxt.Store<Authentication.Credential>().Query.FirstOrDefault();
                var r = cxt.Store<User>().Query;
            }

            Console.ReadKey();
        }
    }

    public class XyzModule: IdentityAccessModuleConfig
    {
        protected override void Initialize()
        {
            base.Initialize();

            this.WithStoreQueryGenerator(_cxt => _cxt.As<DbContext>().Set<User>().OrderBy(_u => _u.UId));

            this.UsingContext(cxt =>
            {
                cxt.Store<User>().Add(new User { EntityId = "Senj@k" });

                cxt.CommitChanges();
            });
        }
    }
}
