using Axis.Jupiter.Europa;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.OAModule;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.RBAC.OAModule;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Axis.Pollux.ConsoleTest
{
    class Program
    {
        public static readonly ContextConfiguration<EuropaContext> Config = new ContextConfiguration<EuropaContext>()
                .WithConnection(ConfigurationManager.ConnectionStrings["EuropaContext"].ConnectionString)
                .WithInitializer(new DropCreateDatabaseIfModelChanges<EuropaContext>())
                .UsingModule(new IdentityAccessModuleConfig())
                .UsingModule(new RBACAccessModuleConfig())
                .UsingModule(new Authentication.OAModule.AuthenticationAccessModuleConfig());


        static void Main(string[] args)
        {
            var finfo = new FileInfo("abcd.xyz");
            var finfo2 = new FileInfo("abcd.xyz");
            Console.WriteLine(finfo == finfo2);
            Console.WriteLine(finfo.Equals(finfo2));

            var config = Config;

            using (var cxt = new EuropaContext(config))
            {
                cxt.Configuration.AutoDetectChangesEnabled = false;
                var user = new User
                {
                    UniqueId = "something@thisway.comes",
                    Status = 1
                };

                //cxt.Add(user).Context.CommitChanges();

                user = new User
                {
                    UniqueId = "something@thisway.comes",
                    Status = 2
                };
                cxt.Modify(user).Context.CommitChanges();

                var ud = new UserData
                {
                    Data = "dafdfa",
                    Name = "here-it",
                    Type = Luna.CommonDataType.String,
                    OwnerId = "something@thisway.comes",
                    //Owner = new User { EntityId = "something@thisway.comes" }
                };
                cxt.Add(ud).Context.CommitChanges();
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
                cxt.Store<User>().Add(new User { UniqueId = "Senj@k" });

                cxt.CommitChanges();
            });
        }
    }
}
