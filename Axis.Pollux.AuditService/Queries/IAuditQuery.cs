using Axis.Luna;
using Axis.Pollux.Audit.Models;
using Axis.Pollux.Common.Models;
using System;

namespace Axis.Pollux.AuditService.Queries
{
    public interface IAuditQuery
    {
        SequencePage<AuditEntry> GetEntries(string userId, PageParams pageParams = null);
        SequencePage<AuditEntry> FindEntries(DateTime start, DateTime end, string userId, PageParams pageParams = null);
    }
}
