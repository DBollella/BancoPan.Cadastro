using System.Text.Json;
using BancoPan.Cadastro.Application.Interfaces;
using StackExchange.Redis;

namespace BancoPan.Cadastro.Infra.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var value = await _database.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        var serialized = JsonSerializer.Serialize(value);

        if (expiration.HasValue)
            await _database.StringSetAsync(key, serialized, expiration.Value);
        else
            await _database.StringSetAsync(key, serialized);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
        var keys = server.Keys(pattern: $"{prefix}*").ToArray();

        if (keys.Length > 0)
            await _database.KeyDeleteAsync(keys);
    }
}