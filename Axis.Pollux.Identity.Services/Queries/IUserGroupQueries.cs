using Axis.Pollux.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.Services.Queries
{
    public interface IUserGroupQueries
    {
        Task<User> GetUserById(Guid userId);
    }
}
