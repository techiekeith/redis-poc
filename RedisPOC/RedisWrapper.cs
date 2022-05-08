using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisPOC;

public class RedisWrapper: IRedisWrapper
{
    private static ConnectionMultiplexer? _redis;
    private readonly IDatabase _db;

    // private readonly string _certificateFile = Path.GetFullPath(
    //     Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificate.crt"));
    
    private readonly string _redisHost = string.Format("{0}:{1}",
        Environment.GetEnvironmentVariable("REDIS_HOST"),
        Environment.GetEnvironmentVariable("REDIS_PORT"));

    
    public RedisWrapper()
    {
        // Console.WriteLine($"Certificate file: {_certificateFile}");
        // var testCert = new X509Certificate2(_certificateFile);
        // Console.WriteLine($"Serial number: {testCert.SerialNumber}");

        var configurationOptions = new ConfigurationOptions
        {
            EndPoints = { _redisHost },
            Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD"),
            Ssl = true
        };
        // configurationOptions.CertificateSelection += delegate
        // {
        //     var cert = new X509Certificate2(_certificateFile);
        //     return cert;
        // };
        // configurationOptions.CertificateValidation += ValidateCertificate;
        _redis ??= ConnectionMultiplexer.Connect(configurationOptions);
        _db = _redis.GetDatabase();
    }
    
    // private static bool ValidateCertificate(
    //     object sender,
    //     X509Certificate? certificate,
    //     X509Chain? chain,
    //     SslPolicyErrors sslPolicyErrors)
    // {
    //     if (sslPolicyErrors == SslPolicyErrors.None)
    //         return true;
    //
    //     Console.WriteLine($"Certificate error: {sslPolicyErrors}");
    //     var cert = certificate as X509Certificate2;
    //     if (cert != null)
    //     {
    //         Console.WriteLine("Cert: {0}", cert.Subject);
    //         var extensionNum = 0;
    //         foreach (var extension in cert.Extensions)
    //         {
    //             extensionNum++;
    //             Console.WriteLine("Extension {0}: {1} = {2}", extensionNum,
    //                 extension.Oid?.FriendlyName ?? "null",
    //                 extension.Oid?.Value ?? "null");
    //         }
    //     }
    //     if (chain != null)
    //     {
    //         var elementNum = 0;
    //         foreach (var element in chain.ChainElements)
    //         {
    //             elementNum++;
    //             Console.WriteLine("Chain {0}: {1}", elementNum, element.Certificate.Subject);
    //             var extensionNum = 0;
    //             foreach (var extension in element.Certificate.Extensions)
    //             {
    //                 extensionNum++;
    //                 Console.WriteLine("Extension {0}:{1}: {2} = {3}",
    //                     elementNum, extensionNum,
    //                     extension.Oid?.FriendlyName ?? "null",
    //                     extension.Oid?.Value ?? "null");
    //             }
    //         }
    //     }
    //
    //     return true;
    // }

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
