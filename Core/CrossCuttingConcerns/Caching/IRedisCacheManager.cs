using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Cache
{
    public interface IRedisCacheManager : ICacheManager
    {
        DistributedCacheEntryOptions SetOptions(int absoluteExpirationRelativeToNow = 20, int slidingExpiration = 20);

        DistributedCacheEntryOptions SetOptions(int absoluteExpiration, int absoluteExpirationRelativeToNow = 20, int slidingExpiration = 20);

        string GetString(string key);

        Task<string> GetStringAsync(string key, CancellationToken token = default(CancellationToken));

        bool SetString(string key, string value);

        Task<bool> SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken));

        Task<bool> ContainsAsync(string key);

        Task<bool> RemoveAsync(string key);

        Task<bool> RemoveByPatternAsync(string pattern);

        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken));

        Task<T> GetAsync<T>(string key, CancellationToken token = default(CancellationToken)) where T : class;

        T GetJson<T>(string key) where T : class;

        Task<T> GetJsonAsync<T>(string key) where T : class;

        bool SetJson<T>(string key, T objectToCache) where T : class;

        Task<bool> SetJsonAsync<T>(string key, T objectToCache) where T : class;

    }
}
