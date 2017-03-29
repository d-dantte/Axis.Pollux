using Axis.Jupiter;
using Axis.Pollux.Account.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.Account.OAModule.Queries
{
    public class AccountQuery: IAccountQuery
    {
        private IDataContext _europa = null;

        public AccountQuery(IDataContext context)
        {
            ThrowNullArguments(() => context);

            _europa = context;
        }
    }
}
