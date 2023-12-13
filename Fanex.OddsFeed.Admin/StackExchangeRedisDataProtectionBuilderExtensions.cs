namespace Fanex.OddsFeed.Admin
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.DataProtection.KeyManagement;
    using Microsoft.AspNetCore.DataProtection.StackExchangeRedis;
    using StackExchange.Redis;

    public static class StackExchangeRedisDataProtectionBuilderExtensions
    {
        private const string DataProtectionKeysName = "DataProtection-Keys";

        public static IDataProtectionBuilder PersistKeysToStackExchangeRedis(this IDataProtectionBuilder builder, Func<IDatabase> databaseFactory, RedisKey key)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (databaseFactory == null)
            {
                throw new ArgumentNullException(nameof(databaseFactory));
            }
            return PersistKeysToStackExchangeRedisInternal(builder, databaseFactory, key);
        }

        public static IDataProtectionBuilder PersistKeysToStackExchangeRedis(this IDataProtectionBuilder builder, IConnectionMultiplexer connectionMultiplexer)
        {
            return PersistKeysToStackExchangeRedis(builder, connectionMultiplexer, DataProtectionKeysName);
        }
        public static IDataProtectionBuilder PersistKeysToStackExchangeRedis(this IDataProtectionBuilder builder, IConnectionMultiplexer connectionMultiplexer, RedisKey key)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (connectionMultiplexer == null)
            {
                throw new ArgumentNullException(nameof(connectionMultiplexer));
            }
            return PersistKeysToStackExchangeRedisInternal(builder, () => connectionMultiplexer.GetDatabase(), key);
        }

        private static IDataProtectionBuilder PersistKeysToStackExchangeRedisInternal(IDataProtectionBuilder builder, Func<IDatabase> databaseFactory, RedisKey key)
        {
            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new RedisXmlRepository(databaseFactory, key);
            });
            return builder;
        }
    }
}
