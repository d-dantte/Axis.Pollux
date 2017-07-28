using Axis.Luna.Operation;
using Axis.Pollux.Audit.Services;
using System;
using Axis.Luna;
using Axis.Pollux.Audit.Models;
using Axis.Jupiter.Commands;
using Axis.Pollux.AuditService.Queries;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Pollux.Identity.Services;
using Axis.Luna.Extensions;

namespace Axis.Pollux.AuditService.Services
{
    public class AuditManager : IAuditManager
    {
        private IPersistenceCommands _pcommand;
        private IAuditQuery _query;
        private IUserContext _userContext;

        public AuditManager(IUserContext userContext, IPersistenceCommands persistenceCommands, IAuditQuery query)
        {
            ThrowNullArguments(() => persistenceCommands,
                               () => query,
                               () => userContext);

            _pcommand = persistenceCommands;
            _query = query;
        }


        public IOperation<SequencePage<AuditEntry>> FindEntries(DateTime start, DateTime end, Pollux.Common.Models.PageParams pageParams = null)
        => LazyOp.Try(() => _query.FindEntries(start, end, null, pageParams));

        public IOperation<SequencePage<AuditEntry>> FindEntries(DateTime start, DateTime end, Pollux.Identity.Principal.User targetUser, Pollux.Common.Models.PageParams pageParams = null)
        => LazyOp.Try(() => _query.FindEntries(start, end, targetUser.UserId, pageParams));

        public IOperation<SequencePage<AuditEntry>> GetEntries(Pollux.Common.Models.PageParams pageParams = null)
        => LazyOp.Try(() => _query.GetEntries(null, pageParams));

        public IOperation<SequencePage<AuditEntry>> GetEntries(Pollux.Identity.Principal.User targetUser, Pollux.Common.Models.PageParams pageParams = null)
        => LazyOp.Try(() => _query.GetEntries(targetUser.UserId, pageParams));

        public IOperation LogEntry(string action, string context)
        => LazyOp.Try(() =>
        {
            new AuditEntry
            {
                Action = action.ThrowIfNull("Invalid audit action"),
                ContextData = context,
                User = _userContext.User()
            }
            .Pipe(_pcommand.Add)
            .Resolve();
        });
    }
}
