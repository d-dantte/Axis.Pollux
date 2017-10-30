using Axis.Jupiter.Europa;
using Axis.Pollux.ABAC.AttributeSources.Models;
using Axis.Pollux.ABAC.AttributeSources.Services;
using Axis.Pollux.ABAC.RolePermissionPolicy.Services;
using Axis.Pollux.ABAC.Services;
using Axis.Pollux.AccountManagement.OAModule;
using Axis.Pollux.Authentication.OAModule;
using Axis.Pollux.Identity.OAModule;
using Axis.Pollux.Identity.Services;
using Axis.Pollux.RoleAuth.OAModule;
using Axis.Pollux.RoleAuth.OAModule.Queries;
using Axis.Pollux.RoleManagement.Services;
using Axis.Sigma.Core.Authority;
using System;
using System.Configuration;
using System.Data.Entity;
using static Axis.Luna.Extensions.EnumerableExtensions;
using System.Linq;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.ABAC.Auth;

namespace Axis.Pollux.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var cc = new ContextConfiguration<DataStore>()
                .WithConnection(ConfigurationManager.ConnectionStrings["EuropaContext"].ConnectionString)
                .WithInitializer(new DropCreateDatabaseIfModelChanges<DataStore>())
                .UsingPolluxIdentityOAModule()
                .UsingPolluxAuthenticationOAModule()
                .UsingPolluxAccountOAModule()
                .UsingPolluxRoleAuthOAModule();

            var store = new DataStore(cc);

            //create the database
            store.Database.CreateIfNotExists();

            //build some intent maps
            var intentMap = new OperationIntentMap()
                .Map<IUserManager>(um => um.ActivateAddress(0),
                                   new AccessIntent("oprx://identity/addresses/", "activate"))
                .Map(typeof(IUserManager).GetMethod(nameof(IUserManager.AddAddressData)),
                     new AccessIntent("oprx://identity/addresses/", "add"));
            var intentSource = new IntentMapSource<IUserManager>(intentMap, um => um.ActivateAddress(0)); //<-- the action we are trying to authorize


            var roleManager = new RoleManager(store, new RoleManagementQueries(store));

            //subject sources
            var usercontext = new FakeUserContext("@root", store);
            var identitySource = new UserIdentitySource(usercontext);
            var userRoleSource = new UserRoleSource(roleManager, usercontext);


            //auth request
            var authReq = new AuthorizationContext(Enumerate<IAttributeSource>(identitySource, userRoleSource), 
                                                   Enumerate(intentSource), 
                                                   null);

            //retrieve the policies
            var x = roleManager.GetAllRoles(null).Resolve();
            var policyReader = new RolePolicyReader(roleManager);
            var authConfig = new AuthorityConfiguration(Enumerate(policyReader));
            var policyAuthority = new PolicyAuthority(authConfig);

            var started = DateTime.Now;
            policyAuthority
                .Authorize(authReq)
                .Then(() => Console.WriteLine("Access Granted!"),
                      ex => Console.WriteLine($"Access Denied"))
                .ResolveSafely();
            Console.WriteLine($"evaluated in {DateTime.Now - started}");

            started = DateTime.Now;
            policyAuthority
                .Authorize(authReq)
                .Then(() => Console.WriteLine("Access Granted!"),
                      ex => Console.WriteLine($"Access Denied"))
                .ResolveSafely();
            Console.WriteLine($"evaluated in {DateTime.Now - started}");

            started = DateTime.Now;
            policyAuthority
                .Authorize(authReq)
                .Then(() => Console.WriteLine("Access Granted!"),
                      ex => Console.WriteLine($"Access Denied"))
                .ResolveSafely();
            Console.WriteLine($"evaluated in {DateTime.Now - started}");

            started = DateTime.Now;
            policyAuthority
                .Authorize(authReq)
                .Then(() => Console.WriteLine("Access Granted!"),
                      ex => Console.WriteLine($"Access Denied"))
                .ResolveSafely();
            Console.WriteLine($"evaluated in {DateTime.Now - started}");


            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }

    public class FakeUserContext : IUserContext
    {
        private User _user;

        public FakeUserContext(string name, DataStore store)
        {
            _user = store.Query<UserEntity>().FirstOrDefault(_u => _u.UniqueId == name).Transform<UserEntity, User>(store);
        }

        public User User() => _user;
    }
}
