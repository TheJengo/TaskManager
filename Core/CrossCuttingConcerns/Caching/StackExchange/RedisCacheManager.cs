using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Extensions;

namespace Utilities.Cache.StackExchange
{
    // TODO - Need to get rid of installing Microsoft.Extensions.Caching.Redis imported project
    public class RedisCacheManager : IRedisCacheManager
    {
        private static CancellationTokenSource _resetCacheToken = new CancellationTokenSource();
        private readonly IDistributedCache _distributedCache;
        private readonly IDatabase _redisDb;

        public RedisCacheManager(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _redisDb = RedisConnectionFactory.RedisCache;
        }

        /// <summary>
        /// Lets you get the options you want to set
        /// </summary>
        /// <param name="absoluteExpirationRelativeToNow"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public DistributedCacheEntryOptions SetOptions(int absoluteExpirationRelativeToNow = 20, int slidingExpiration = 20)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationRelativeToNow),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
            };
        }

        /// <summary>
        /// Lets you set the options you want to get
        /// </summary>
        /// <param name="absoluteExpiration"></param>
        /// <param name="absoluteExpirationRelativeToNow"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public DistributedCacheEntryOptions SetOptions(int absoluteExpiration, int absoluteExpirationRelativeToNow = 20, int slidingExpiration = 20)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow, TimeSpan.FromMinutes(absoluteExpiration)),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationRelativeToNow),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
            };
        }

        public string GetString(string key)
        {
            return _distributedCache.GetString(key);
        }

        public Task<string> GetStringAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return _distributedCache.GetStringAsync(key, token);
        }

        public bool SetString(string key, string value)
        {
            try
            {
                _distributedCache.SetString(key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            try
            {
                await _distributedCache.SetStringAsync(key, value, options, token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Contains(string key)
        {
            try
            {
                if (_redisDb == null) return false;

                return _redisDb.KeyExists(key);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ContainsAsync(string key)
        {
            try
            {
                if (_redisDb == null) return false;

                return await _redisDb.KeyExistsAsync(key);
            }
            catch
            {
                return false;
            }
        }

        public void Remove(string key)
        {
            if (_redisDb == null)
                return;

            _redisDb.KeyDelete(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                if (_redisDb == null) return false;

                return await _redisDb.KeyDeleteAsync(key);
            }
            catch
            {
                return false;
            }
        }

        //SetSlidingExpiration(): Redis’de ilgili key ile belli bir zaman hiçbir işlem yapılmaz ise, cache’in düşeceğinin belirlenmesidir.
        //AbsoluteExpirationRelativeToNow(): Redis’de ilgili keye ait cache’in mutlaka yani işlem yapılsın ya da yapılmasın belirli zaman sonra düşeceğinin belirlenmesidir.
        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            await _distributedCache.SetAsync(key, value.ToByteArray(), options, token);
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default(CancellationToken)) where T : class
        {
            var result = await _distributedCache.GetAsync(key, token);
            return result.FromByteArray<T>();
        }

        /// <summary>
        /// Sets cache entry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        public void Set<T>(string key, T value, int duration = 20)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(duration));
            _distributedCache.Set(key, value.ToByteArray(), options);
        }

        /// <summary>
        /// Returns cache entry
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            var result = _distributedCache.Get(key);
            return result.FromByteArray<T>();
        }

        public T GetJson<T>(string key) where T : class
        {
            try
            {
                if (_redisDb == null) return (T)null;

                var redisObject = _redisDb.StringGet(key);
                if (redisObject.HasValue)
                {
                    return JsonConvert.DeserializeObject<T>(redisObject
                            , new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                                PreserveReferencesHandling = PreserveReferencesHandling.Objects
                            });
                }
                else
                {
                    return (T)null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<T> GetJsonAsync<T>(string key) where T : class
        {
            try
            {
                if (_redisDb == null) return (T)null;

                var redisObject = await _redisDb.StringGetAsync(key);
                if (redisObject.HasValue)
                {
                    return JsonConvert.DeserializeObject<T>(redisObject
                            , new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                                PreserveReferencesHandling = PreserveReferencesHandling.Objects
                            });
                }
                else
                {
                    return (T)null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool SetJson<T>(string key, T objectToCache) where T : class
        {
            try
            {
                if (_redisDb == null) return false;

                _redisDb.StringSet(key, JsonConvert.SerializeObject(objectToCache
                        , Formatting.Indented
                        , new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects
                        }));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetJsonAsync<T>(string key, T objectToCache) where T : class
        {
            try
            {
                if (_redisDb == null) return false;

                await _redisDb.StringSetAsync(key, JsonConvert.SerializeObject(objectToCache
                        , Formatting.Indented
                        , new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects
                        }));

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Deletes cached entries who had a match with the pattern
        /// </summary>
        /// <param name="pattern">String</param>
        /// <returns></returns>
        public void RemoveByPattern(string pattern)
        {
            try
            {
                var multiplexer = RedisConnectionFactory.Connection;
                var endPoints = multiplexer.GetEndPoints();

                foreach (var ep in endPoints)
                {
                    var server = multiplexer.GetServer(ep);

                    var keys = server.Keys(database: _redisDb.Database, pattern: pattern + "*").ToArray();

                    _redisDb.KeyDelete(keys);
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
		/// Deletes asynchronously cached entries who had a match with the pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<bool> RemoveByPatternAsync(string pattern)
        {
            try
            {
                var multiplexer = RedisConnectionFactory.Connection;
                var endPoints = multiplexer.GetEndPoints();

                foreach (var ep in endPoints)
                {
                    var server = multiplexer.GetServer(ep);

                    var keys = server.Keys(database: _redisDb.Database, pattern: pattern + "*").ToArray();

                    await _redisDb.KeyDeleteAsync(keys);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// expires cache entries T based on CancellationTokenSource cancel 
        /// </summary>
        public void Reset()
        {
            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested &&
                _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }
            _resetCacheToken = new CancellationTokenSource();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
