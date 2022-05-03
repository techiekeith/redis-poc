namespace RedisPOC;

public interface IRedisWrapper
{
    bool Exists(string key);
    T? Get<T>(string key);
    bool Set(string key, object? value);
    long Del(IEnumerable<string> keys);
    Task<bool> ExistsAsync(string key);
    Task<T?> GetAsync<T>(string key);
    Task<bool> SetAsync(string key, object? value);
    Task<long> DelAsync(IEnumerable<string> keys);
}
