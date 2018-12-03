using System;
using System.Threading.Tasks;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Services.Queries
{
    public interface IUserQueries
    {
        Task<User> GetUserById(Guid userId);
        Task<long> UserCount();
    }
}
