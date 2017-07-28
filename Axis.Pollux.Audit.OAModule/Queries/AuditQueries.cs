using Axis.Pollux.AuditService.Queries;
using System;
using System.Linq;
using Axis.Luna;
using Axis.Pollux.Audit.Models;
using Axis.Pollux.Common.Models;
using Axis.Jupiter.Europa;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Pollux.Audit.OAModule.Entities;

namespace Axis.Pollux.Audit.OAModule.Queries
{
    public class AuditQueries : IAuditQuery
    {
        private DataStore _store;

        public AuditQueries(DataStore store)
        {
            ThrowNullArguments(() => store);

            this._store = store;
        }

        public SequencePage<AuditEntry> FindEntries(DateTime start, DateTime end, string userId, PageParams pageParams = null)
        {
            var q = _store
                .Query<AuditEntryEntity>(_ae => _ae.User)                
                .Where(_ae => start <= _ae.CreatedOn)
                .Where(_ae => _ae.CreatedOn <= end);

            if (!string.IsNullOrWhiteSpace(userId)) q = q.Where(_ae => _ae.UserId == userId);

            var oq = q.OrderByDescending(_ae => _ae.CreatedOn);

            pageParams = pageParams ?? PageParams.EntireSequence();

            return pageParams.Paginate(oq, _q => _q.Transform<AuditEntryEntity, AuditEntry>(_store));
        }

        public SequencePage<AuditEntry> GetEntries(string userId, PageParams pageParams = null)
        {
            var q = _store.Query<AuditEntryEntity>(_ae => _ae.User);

            if (!string.IsNullOrWhiteSpace(userId)) q = q.Where(_ae => _ae.UserId == userId);

            var oq = q.OrderByDescending(_ae => _ae.User);

            pageParams = pageParams ?? PageParams.EntireSequence();

            return pageParams.Paginate(oq, _q => _q.Transform<AuditEntryEntity, AuditEntry>(_store));
        }
    }
}
