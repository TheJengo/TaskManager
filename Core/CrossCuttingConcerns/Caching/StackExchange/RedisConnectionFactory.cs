using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Cache.StackExchange
{
    public class RedisConnectionFactory
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisConnectionFactory()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { config["AppSettings:Cache:RedisConfig"] }
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        //public static ConnectionMultiplexer Connection => LazyConnection.Value;
        public static ConnectionMultiplexer Connection => LazyConnection.IsValueCreated != false ? LazyConnection.Value : null;

        public static IDatabase RedisCache => Connection != null ? Connection.GetDatabase() : null;
    }
}
