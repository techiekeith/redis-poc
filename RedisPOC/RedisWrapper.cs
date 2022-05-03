using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisPOC;

public class RedisWrapper: IRedisWrapper
{
    private static ConnectionMultiplexer? _redis;
    private readonly IDatabase _db;
    
    public RedisWrapper()
    {
        _redis ??= ConnectionMultiplexer.Connect("localhost:6379");
        _db = _redis.GetDatabase();
    }

    public bool Exists(string key)
    {
        return _db.KeyExists(key);
    }

    public T? Get<T>(string key)
    {
        var redisValue = _db.StringGet(key).ToString();
        return redisValue == null ? default : JsonConvert.DeserializeObject<T>(redisValue);
    }

    public bool Set(string key, object? value)
    {
        return _db.StringSet(key, JsonConvert.SerializeObject(value));
    }

    public long Del(IEnumerable<string> keys)
    {
        return _db.KeyDelete(keys.Select(x => new RedisKey(x)).ToArray());
    }
    
    public async Task<bool> ExistsAsync(string key)
    {
        return await _db.KeyExistsAsync(key);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var redisValue = (await _db.StringGetAsync(key)).ToString();
        return redisValue == null ? default : JsonConvert.DeserializeObject<T>(redisValue);
    }

    public async Task<bool> SetAsync(string key, object? value)
    {
        return await _db.StringSetAsync(key, JsonConvert.SerializeObject(value));
    }

    public async Task<long> DelAsync(IEnumerable<string> keys)
    {
        return await _db.KeyDeleteAsync(keys.Select(x => new RedisKey(x)).ToArray());
    }
}
