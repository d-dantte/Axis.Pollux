using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna.Extensions;

using Axis.Pollux.ABAC.Services;
using System;
using System.Linq;
using Axis.Luna;
using Axis.Pollux.ABAC.Auth;
using Axis.Jupiter;

namespace Axis.Pollux.AttributeAuthorization.Services
{
    public class AttributeStore : IAttributeStore
    {
        private IDataContext _context { get; set; }
        private IAttributeStore _attributeStore { get; set; }


        public AttributeStore(IDataContext dataContext)
        {
            ThrowNullArguments(() => dataContext);

            _context = dataContext;
        }


        public Operation<IAttributeStore> Add(AuthorizationAttribute attribute)
            => Operation.Run(() =>
            {
                _context.Store<AuthorizationAttribute>().Add(attribute);
                _context.CommitChanges();
                return this.As<IAttributeStore>();
            });

        public Operation Delete(AuthorizationAttribute attribute)
            => Operation.Run(() =>
            {
                _context.Store<AuthorizationAttribute>().Delete(attribute, true);
            });


        public Operation<AuthorizationAttribute> Modify(AuthorizationAttribute dobj)
            => Operation.Run(() =>
            {
                _context.Store<AuthorizationAttribute>().Modify(dobj);
                _context.CommitChanges();
                return dobj;
            });

        public IQueryable<AuthorizationAttribute> ResourceAttributes()
            => _context.Store<AuthorizationAttribute>().Query.Where(att => att.Category.Name == Category.Resource.Name);

        public IQueryable<AuthorizationAttribute> SubjectAttributes()
            => _context.Store<AuthorizationAttribute>().Query.Where(att => att.Category.Name == Category.Subject.Name);

        public IQueryable<AuthorizationAttribute> ActionAttributes()
            => _context.Store<AuthorizationAttribute>().Query.Where(att => att.Category.Name == Category.Action.Name);

        public IQueryable<AuthorizationAttribute> EnvironmentAttributes()
            => _context.Store<AuthorizationAttribute>().Query.Where(att => att.Category.Name == Category.Environment.Name);
    }
}
