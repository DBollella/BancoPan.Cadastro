using System.Text.Json;
using BancoPan.Cadastro.Application.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace BancoPan.Cadastro.Infra.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase? _database;
    private readonly bool _isAvailable;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IConnectionMultiplexer? redis, ILogger<RedisCacheService> logger)
    {
        _logger = logger;
        try
        {
            if (redis != null && redis.IsConnected)
            {
                _database = redis.GetDatabase();
                _isAvailable = true;
                _logger.LogInformation("Cache Redis disponível e conectado");
            }
            else
            {
                _isAvailable = false;
                _logger.LogWarning("Cache Redis não disponível - aplicação funcionará sem cache");
            }
        }
        catch (Exception ex)
        {
            _isAvailable = false;
            _logger.LogWarning(ex, "Erro ao inicializar Redis - aplicação funcionará sem cache");
        }
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        if (!_isAvailable || _database == null)
        {
            _logger.LogDebug("Cache não disponível para leitura da chave: {Key}", key);
            return null;
        }

        try
        {
            var value = await _database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return null;

            _logger.LogDebug("Cache hit para chave: {Key}", key);
            return JsonSerializer.Deserialize<T>(value!);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao ler do cache a chave: {Key}", key);
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        if (!_isAvailable || _database == null)
        {
            _logger.LogDebug("Cache não disponível para escrita da chave: {Key}", key);
            return;
        }

        try
        {
            var serialized = JsonSerializer.Serialize(value);

            if (expiration.HasValue)
                await _database.StringSetAsync(key, serialized, expiration.Value);
            else
                await _database.StringSetAsync(key, serialized);

            _logger.LogDebug("Dados gravados no cache para chave: {Key} (Expiração: {Expiration})",
                key, expiration?.ToString() ?? "Sem expiração");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao gravar no cache a chave: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        if (!_isAvailable || _database == null)
        {
            _logger.LogDebug("Cache não disponível para remoção da chave: {Key}", key);
            return;
        }

        try
        {
            await _database.KeyDeleteAsync(key);
            _logger.LogDebug("Chave removida do cache: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao remover do cache a chave: {Key}", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        if (!_isAvailable || _database == null)
        {
            _logger.LogDebug("Cache não disponível para remoção por prefixo: {Prefix}", prefix);
            return;
        }

        try
        {
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{prefix}*").ToArray();

            if (keys.Length > 0)
            {
                await _database.KeyDeleteAsync(keys);
                _logger.LogDebug("Removidas {Count} chaves do cache com prefixo: {Prefix}", keys.Length, prefix);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao remover chaves do cache com prefixo: {Prefix}", prefix);
        }
    }
}