using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisPOC;

public class RedisWrapper: IRedisWrapper
{
    private static ConnectionMultiplexer? _redis;
    private readonly IDatabase _db;
    private readonly string _redisHost = string.Format("{0}:{1}",
        Environment.GetEnvironmentVariable("REDIS_HOST"),
        Environment.GetEnvironmentVariable("REDIS_PORT"));

    
    public RedisWrapper()
    {
        var configurationOptions = new ConfigurationOptions
        {
            EndPoints = { _redisHost },
            Ssl = false,
            AbortOnConnectFail = false,
            ConnectTimeout = 1000,
            SyncTimeout = 1000
        };
        _redis ??= ConnectionMultiplexer.Connect(configurationOptions);
        _db = _redis.GetDatabase();
    }
    
    public bool Exists(string key) => _db.KeyExists(key);

    public T? Get<T>(string key) => Deserialize<T>(_db.StringGet(key));

    public bool Set(string key, object? value) => _db.StringSet(key, JsonConvert.SerializeObject(value));

    public bool SetIfNotExists(string key, object? value) => _db.StringSet(
        key: key,
        value: JsonConvert.SerializeObject(value),
        when: When.NotExists);

    public long Del(IEnumerable<string> keys) => _db.KeyDelete(keys.Select(x => new RedisKey(x)).ToArray());
    
    public async Task<bool> ExistsAsync(string key) => await _db.KeyExistsAsync(key);

    public async Task<T?> GetAsync<T>(string key) => Deserialize<T>(await _db.StringGetAsync(key));

    public async Task<bool> SetAsync(string key, object? value) => await _db.StringSetAsync(key, JsonConvert.SerializeObject(value));

    public async Task<bool> SetIfNotExistsAsync(string key, object? value) => await _db.StringSetAsync(
        key: key,
        value: JsonConvert.SerializeObject(value),
        when: When.NotExists);

    public async Task<long> DelAsync(IEnumerable<string> keys) => await _db.KeyDeleteAsync(keys.Select(x => new RedisKey(x)).ToArray());

    private T? Deserialize<T>(RedisValue redisValue) => redisValue == RedisValue.Null ? default : JsonConvert.DeserializeObject<T>(redisValue.ToString());
}
