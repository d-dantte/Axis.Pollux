using System;
using System.Collections.Generic;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Contracts;

using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Common.Caching.Castor
{
    public class MultilevelCache: ICache
    {
        private readonly ICache[] _cacheLevels = null;

        public event EventHandler<string> Invalidated;

        public MultilevelCache(IEnumerable<ICache> cacheLevels)
        {
            ThrowNullArguments(() => cacheLevels);

            _cacheLevels = cacheLevels
                .ThrowIf(cache => cache.Any(c => c == null), new Exception("Invalid Cache Instance"))
                .ToArray()
                .ThrowIf(caches => caches.Length == 0, new Exception("Cache collection cannot be empty"));
        }

        public ICache Set<TData>(
            string cacheKey, 
            TimeSpan? expiration, 
            Func<string, Operation<TData>> valueProvider) => Set(cacheKey, true, expiration, valueProvider);

        public ICache Set<TData>(
            string cacheKey,
            bool updateIfPresent,
            TimeSpan? expiration,
            Func<string, Operation<TData>> valueProvider)
        {
            if(string.IsNullOrWhiteSpace(cacheKey))
                throw new InvalidCacheKeyException();

            if (valueProvider == null)
                throw new InvalidDataProviderException();

            var provider = valueProvider;
            //in reverse order, traverse the cache list setting up data providers that rely on the cache below it
            //for it's value, with the lowest cache relying on the actual value provider function given
            for (var cnt = _cacheLevels.Length - 1; cnt >= 0; cnt--)
            {
                var cache = _cacheLevels[cnt];
                if (cnt < _cacheLevels.Length - 1)
                {
                    var prev = cnt + 1;
                    provider = (key) => _cacheLevels[prev].GetOrRefresh<TData>(key);
                }

                cache.Set(cacheKey, updateIfPresent, expiration, provider);
            }

            return this;
        }

        public Operation<TData> Get<TData>(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
                return Operation.Fail<TData>(new InvalidCacheKeyException());

            foreach (var cache in _cacheLevels)
            {
                var opdata = cache
                    .Get<TData>(cacheKey)
                    .Wait(); //resolve the operation without throwing exceptions

                if (opdata.Succeeded == true)
                    return opdata;

                //any exception other than "UnpairedCacheKeyException" should disrupt this process
                else if (opdata.Error.GetException().GetType() != typeof(UnpairedCacheKeyException))
                    return opdata;
            }

            throw new UnpairedCacheKeyException(cacheKey);
        }

        public Operation<TData> GetOrRefresh<TData>(string cacheKey)
        => _cacheLevels[0].GetOrRefresh<TData>(cacheKey);

        public Operation<TData> Refresh<TData>(string cacheKey)
        => _cacheLevels[0].Refresh<TData>(cacheKey);

        public Operation Invalidate(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
                return Operation.Fail(new InvalidCacheKeyException());

            return _cacheLevels
                .Select(cache => cache.Invalidate(cacheKey))
                .Fold();
        }

        public Operation InvalidateAll()
        {
            return _cacheLevels
                .Select(cache => cache.InvalidateAll())
                .Fold();
        }

        protected virtual void OnInvalidated(string cacheKey)
        {
            if(string.IsNullOrWhiteSpace(cacheKey))
                throw new InvalidCacheKeyException();

            Invalidated?.Invoke(this, cacheKey);
        }
    }
}
