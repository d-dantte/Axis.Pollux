using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Axis.Pollux.Authentication.Models;

namespace Axis.Pollux.Authentication.Services.Queries
{
    public interface IMultiFactorConfigurationQueries
    {

        Task<MultiFactorEventConfiguration> GetMultiFactorConfiguration(string eventLabel);
    }
}
