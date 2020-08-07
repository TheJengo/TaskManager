using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface IMemoryCacheManager : ICacheManager
    {
        MemoryCacheEntryOptions SetOptions(short priority = 1, int slidingExpiration = 20, int absoluteExpiration = 20);
        T Set<T>(object key, T value, MemoryCacheEntryOptions options);
    }
}
