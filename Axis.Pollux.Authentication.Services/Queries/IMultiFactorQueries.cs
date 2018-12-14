using System;
using System.Threading.Tasks;
using Axis.Pollux.Authentication.Models;

namespace Axis.Pollux.Authentication.Services.Queries
{
    public interface IMultiFactorQueries
    {
        /// <summary>
        /// Gets active (not expired or authenticated) credentials meeting the filter criteria
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventLabel"></param>
        /// <returns></returns>
        Task<MultiFactorCredential> GetActiveCredential(Guid userId, string eventLabel);
    }
}
