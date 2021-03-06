﻿using System;
using Axis.Luna.Operation;

namespace Axis.Pollux.Common.Contracts
{
    /// <summary>
    /// Caching abstraction for the Pollux Library. Unlike conventional caches, this cache does not receive data values
    /// from external sources to pair with keys, rather, it accepts a value provider that in turn generates the values
    /// when needed. Values are not generated upon setting the provider, but rather upon requesting the data via the key,
    /// and upon first call, the value is cached so subsequent calls return the cached value. Validity expiration can
    /// also be set for the value provider, and these begin ticking when the value provider is queried. After the
    /// timer expires, the cached value is deleted. If a null expiration is specified, the value never expires. Based on
    /// implementation, other reasons could exist internally for the cache entry (key-value pair) to be invalidated.
    /// </summary>
    public interface ICache
    {
        event EventHandler<string> Invalidated;

        /// <summary>
        /// Adds or updates the data provider for a specific key in the cache, as well as adding an invalidation timer.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="expiration"></param>
        /// <param name="valueProvider"></param>
        /// <returns></returns>
        ICache Set<TData>(string cacheKey, TimeSpan? expiration, Func<string, Operation<TData>> valueProvider);

        /// <summary>
        /// Adds or updates the data provider for a specific key in the cache, as well as adding an invalidation timer.
        /// Updating the provider and expiration depends on the value of the'updateIfPresent' flag.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="updateIfPresent"></param>
        /// <param name="expiration"></param>
        /// <param name="valueProvider"></param>
        /// <returns></returns>
        ICache Set<TData>(string cacheKey, bool updateIfPresent, TimeSpan? expiration, Func<string, Operation<TData>> valueProvider);

        /// <summary>
        /// Retrieves a value from the cache.
        /// If no Value provider is set, an exception is thrown (wrapped in an operation).
        /// if the key has been invalidated, an exception is thrown (wrapped in an operation).
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Operation<TData> Get<TData>(string cacheKey);

        /// <summary>
        /// Gets a cached value: if non exists, or the key is invalidated or expired, attempt to query the value provider 
        /// for a value and reset the expiration timer (if one was provided). If no value provider
        /// exists, return a failed operation containing an exception
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Operation<TData> GetOrRefresh<TData>(string cacheKey);

        /// <summary>
        /// Query the value provider for a new value and reset the timer if one was provided, returning a failed operation if no
        /// provider exists for that key
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Operation<TData> Refresh<TData>(string cacheKey);

        /// <summary>
        /// Force the cache to delete the value associated with a key
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Operation Invalidate(string cacheKey);

        /// <summary>
        /// Forces the cache to delete the values associated with all keys
        /// </summary>
        /// <returns></returns>
        Operation InvalidateAll();
    }


    /// <summary>
    /// Thrown when an invalid key (null) is used to interact with the cache
    /// </summary>
    public class InvalidCacheKeyException : Exception
    {
    }

    /// <summary>
    /// Thrown when an invalid key (null) is used to interact with the cache
    /// </summary>
    public class InvalidDataProviderException : Exception
    {
    }

    /// <summary>
    /// Thrown when no value provider exists in the cache for the given key and a value is requested.
    /// Note that if a key exists, a value provider MUST exist also.
    /// </summary>
    public class UnpairedCacheKeyException : Exception
    {
        public string Key { get; }

        public UnpairedCacheKeyException(string key)
        : base($"No Value Provider found for the key: {key}")
        {
            Key = key;
        }
    }

    /// <summary>
    /// If a value provider throws an exception while attempting to generate/retrieve the value
    /// </summary>
    public class ValueProviderResolutionException : Exception
    {
        public ValueProviderResolutionException(Exception innerException)
        : base("ValueProvider Resolution Failure", innerException)
        { }
    }

}
