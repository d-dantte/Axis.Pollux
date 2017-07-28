using Axis.Luna;
using Axis.Luna.Operation;
using Axis.Pollux.Audit.Models;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Identity.Principal;
using System;

namespace Axis.Pollux.Audit.Services
{
    public interface IAuditManager
    {
        IOperation LogEntry(string action, string context);


        IOperation<SequencePage<AuditEntry>> GetEntries(PageParams pageParams = null);
        IOperation<SequencePage<AuditEntry>> FindEntries(DateTime start, DateTime end, PageParams pageParams = null);

        IOperation<SequencePage<AuditEntry>> GetEntries(User targetUser, PageParams pageParams = null);
        IOperation<SequencePage<AuditEntry>> FindEntries(DateTime start, DateTime end, User targetUser, PageParams pageParams = null);
    }
}
