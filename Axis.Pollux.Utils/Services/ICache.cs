using System;
using Axis.Luna.Operation;

namespace Axis.Pollux.Utils.Services
{
    public interface ICache
    {
        IOperation<Data> GetOrAdd<Data>(string cacheKey, Func<string, Data> dataProvider) where Data : class;

        IOperation<Data> GetOrRefresh<Data>(string cacheKey) where Data : class;

        IOperation<Data> Get<Data>(string cacheKey) where Data : class;

        IOperation Invalidate(string cacheKey);

        IOperation InvalidateAll();

        IOperation<Data> Refresh<Data>(string cacheKey) where Data : class;
    }
}
