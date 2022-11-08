using Medallion.Threading;
using Medallion.Threading.Redis;
using StackExchange.Redis;

namespace WebApplication1;

public static class Extensions
{
    public static IServiceCollection AddRedisDistributedLock(this IServiceCollection service,
        IConfiguration configuration)
        => service.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connectionString = configuration.GetConnectionString("redis");
            // var configurationOptions = connectionString != null
            //     ? ConfigurationOptions.Parse(connectionString)
            //     : throw new ArgumentNullException(nameof(connectionString));
            //
            // var db = configurationOptions.DefaultDatabase.GetValueOrDefault();

            var connection = ConnectionMultiplexer
                .Connect(connectionString);
            return new
                RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });
}