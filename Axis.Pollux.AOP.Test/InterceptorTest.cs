using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Proteus;
using SimpleInjector;
using System.Collections.Generic;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Pollux.AOP.ServiceRegistry;
using Axis.Pollux.AOP.ServiceRegistry.Interceptors;
using Axis.Pollux.ABAC.AttributeSources.Models;
using Axis.Sigma.Core.Policy;
using Axis.Sigma.Core.Authority;
using Axis.Pollux.RoleManagement.Queries;
using Axis.Luna;
using Axis.Pollux.RoleAuth.Models;
using Axis.Pollux.Common.Models;
using Axis.Luna.Extensions;
using Axis.Pollux.ABAC.RolePermissionPolicy.Services;
using Axis.Pollux.RoleAuth.Services;
using Axis.Pollux.RoleManagement.Services;
using System.Reflection;
using Axis.Pollux.ABAC.AttributeSources.Services;
using Axis.Pollux.ABAC.Auth;
using Axis.Jupiter.Commands;
using Axis.Pollux.ABAC.Services;
using Axis.Sigma.Core;
using Axis.Luna.Utils;
using Axis.Pollux.ABAC;

namespace Axis.Pollux.AOP.Test
{
    [TestClass]
    public class InterceptorTest
    {
        [TestMethod]
        public void LazyInvocation()
        {
            var container = new SampleContainer();
            container
                .BindInterceptorFor<ISample, Sample>(null, () => new LazyServiceLoader<Sample>(container))
                .BindInterceptorFor<ISample2, Sample2>(null, () => new LazyServiceLoader<Sample>(container));

            var x = container.Resolve<ISample>();
            Assert.IsNotNull(x);

            var op = x.Method1();
            Assert.IsNotNull(op);

            op.Then(_v =>
              {
                  Assert.AreEqual("called", _v);
              }, err => Assert.Fail(err.Message))
              .Resolve();
        }


        [TestMethod]
        public void ServiceOperationAuthorizerTest()
        {
            var intentMap = new OperationIntentMap()
                .Map(typeof(ISample).GetMethod("Method1"), new AccessIntent("oprx://domain/samples/", "get"));
            
            var container = new SampleContainer();
            container
                .BindInterceptorFor<ISample, Sample>(null, () => new ServiceOperationAuthorizer(container, Authorize), () => new LazyServiceLoader<Sample>(container))
                .BindInterceptorFor<ISample2, Sample2>(null, () => new LazyServiceLoader<Sample>(container))
                .Register(() => new AuthorityConfiguration(new[] { container.Resolve<IPolicyReader>() }))
                .Register<PolicyAuthority>()
                .Register<IPolicyReader, RolePolicyReader>()
                .Register<IRoleManager, RoleManager>()
                .Register<IRoleManagementQueries, RoleQuery>()
                .Register<IPersistenceCommands, XCommands>()
                .Register(() => intentMap);

            var x = container.Resolve<ISample>();
            Assert.IsNotNull(x);

            var op = x.Method1();
            Assert.IsNotNull(op);
            op.Then(_v =>
            {
                Assert.AreEqual("called", _v);
            }, err => Assert.Fail(err.Message))
            .Resolve();

            var start = DateTime.Now;
            x.Method1().Resolve();
            Console.WriteLine($"completed in: {DateTime.Now - start}");
        }

        public IOperation Authorize(IServiceResolver resolver, MethodInfo minfo)
        => LazyOp.Try(() =>
        {
            var imap = resolver.Resolve<OperationIntentMap>();
            var s = new IntentMapSource(imap, minfo);
            var cxt = new AuthorizationContext(new UserRoleSource(), s, new EmptyAttributeSource());

            var authority = resolver.Resolve<PolicyAuthority>();
            return authority.Authorize(cxt);
        });
    }

    public interface ISample
    {
        IOperation<string> Method1();
    }

    public interface ISample2
    {
        IOperation<string> Method2();
    }


    public class Sample: ISample
    {
        public ISample2 _sample;
        public Sample(ISample2 x)
        {
            _sample = x;
        }

        public IOperation<string> Method1()
        => LazyOp.Try(() =>
        {
            Console.WriteLine("Called");
            return "called";
        });
    }

    public class Sample2: ISample2
    {
        public ISample _sample;
        public Sample2(ISample x)
        {
            _sample = x;
        }

        public IOperation<string> Method2()
        => LazyOp.Try(() =>
        {
            Console.WriteLine("Called");
            return "called";
        });
    }


    public class SampleContainer : IServiceResolver, IServiceRegistrar
    {
        private Container _container = new Container();

        public void Dispose()
        {
            _container.Dispose();
        }

        #region IServiceResolver
        public object Resolve(Type serviceType, params object[] args) => _container.GetInstance(serviceType);

        public Service Resolve<Service>(params object[] args) => (Service)_container.GetInstance(typeof(Service));

        public IEnumerable<object> ResolveAll(Type serviceType, params object[] args) => _container.GetAllInstances(serviceType);

        public IEnumerable<Service> ResolveAll<Service>(params object[] args) => _container.GetAllInstances(typeof(Service)).Cast<Service>();
        #endregion

        #region IServiceRegistrar
        public IServiceRegistrar Register(Type concreteType, object param = null)
        {
            _container.Register(concreteType);
            return this;
        }

        public IServiceRegistrar Register(Type serviceType, Func<object> factory, object param = null)
        {
            if (param == null) _container.Register(serviceType, factory);
            else _container.Register(serviceType, factory, (Lifestyle)param);

            return this;
        }

        public IServiceRegistrar Register(Type serviceType, Type concreteType, object param = null)
        {
            if (param == null) _container.Register(serviceType, concreteType);
            else _container.Register(serviceType, concreteType, (Lifestyle)param);
            return this;
        }

        public IServiceRegistrar Register<Impl>(object param = null)
        where Impl : class
        {
            if (param == null) _container.Register<Impl>();
            else _container.Register<Impl>((Lifestyle)param);
            return this;
        }

        public IServiceRegistrar Register<Service>(Func<Service> factory, object param = null)
        where Service : class
        {
            if (param == null) _container.Register(factory);
            else _container.Register(factory, (Lifestyle)param);
            return this;
        }

        public IServiceRegistrar Register<Service, Impl>(object param = null)
        where Service : class
        where Impl : class, Service
        {
            if (param == null) _container.Register<Service, Impl>();
            else _container.Register<Service, Impl>((Lifestyle)param);
            return this;
        }
        #endregion
    }

    public class UserRoleSource : IAttributeSource
    {
        public IOperation<IEnumerable<IAttribute>> GetAttributes()
        => LazyOp.Try(() =>
        {
            return new[]
            {
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_UserRole,
                    Data = "#root"
                }
            }
            .AsEnumerable<IAttribute>();
        });
    }

    public class RoleQuery : IRoleManagementQueries
    {
        private List<RolePermission> _permissions = new List<RolePermission>();

        public RoleQuery()
        {
            _permissions.Add(new RolePermission
            {
                CreatedOn = DateTime.Now,
                Effect = PermissionEffect.Grant,
                Label = "Fake",
                Resource = "oprx://domain/samples/@get",
                UniqueId = 1,
                UUID = Guid.NewGuid(),
                Role = new Role
                {
                    RoleName = "#root",
                    Status = RoleStatus.Enabled
                }
            });
        }

        public SequencePage<Role> GetAllRoles(PageParams pageParams = null)
        {
            return new SequencePage<Role>(_permissions.Select(_rp => _rp.Role).ToArray(), 1, 1, 0);
        }

        public RolePermission GetPermissionForUUID(Guid uuid)
        {
            return _permissions.FirstOrDefault(_p => _p.UUID == uuid);
        }

        public SequencePage<RolePermission> GetPermissionsForLabel(string label, PageParams pageParams = null)
        {
            return _permissions
                .Where(_p => _p.Label == label)
                .Pipe(_par =>
                {
                    return new SequencePage<RolePermission>(_par.ToArray(), _par.Count(), _par.Count(), 0);
                });
        }

        public SequencePage<RolePermission> GetPermissionsForResource(string resource, PageParams pageParams = null)
        {
            return _permissions
                .Where(_p => _p.Resource == resource)
                .Pipe(_par =>
                {
                    return new SequencePage<RolePermission>(_par.ToArray(), _par.Count(), _par.Count(), 0);
                });
        }

        public SequencePage<RolePermission> GetPermissionsForRole(string roleName, PageParams pageParams = null)
        {
            return _permissions
                .Where(_p => _p.Role.RoleName == roleName)
                .Pipe(_par =>
                {
                    return new SequencePage<RolePermission>(_par.ToArray(), _par.Count(), _par.Count(), 0);
                });
        }

        public UserRole GetUserRole(string userId, string roleName)
        {
            return null;
        }

        public SequencePage<UserRole> GetUserRolesFor(string userId, PageParams pageParams = null)
        {
            return new SequencePage<UserRole>();
        }
    }

    public class XCommands : IPersistenceCommands
    {
        public IOperation<Model> Add<Model>(Model d) where Model : class
        {
            throw new NotImplementedException();
        }

        public IOperation AddBatch<Model>(IEnumerable<Model> d, int batchSize = 0) where Model : class
        {
            throw new NotImplementedException();
        }

        public IOperation<Model> Delete<Model>(Model d) where Model : class
        {
            throw new NotImplementedException();
        }

        public IOperation DeleteBatch<Model>(IEnumerable<Model> d, int batchSize = 0) where Model : class
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IOperation<Model> Update<Model>(Model d) where Model : class
        {
            throw new NotImplementedException();
        }

        public IOperation UpdateBatch<Model>(IEnumerable<Model> d, int batchSize = 0) where Model : class
        {
            throw new NotImplementedException();
        }
    }
}
