using Newtonsoft.Json;
using RedisPOC;

if (args.Length < 2)
{
    Syntax();
}

var redis = new RedisWrapper();
switch (args[0])
{
    case "exists":
        var exists = redis.Exists(args[1]);
        Console.WriteLine(exists ? "Record found." : "Record not found.");
        break;
    case "exists-async":
        var existsAsync = redis.Exists(args[1]);
        Console.WriteLine(existsAsync ? "Record found." : "Record not found.");
        break;
    case "get":
        var value = redis.Get<object>(args[1]);
        Console.WriteLine(value ?? "<null>");
        break;
    case "get-async":
        var valueAsync = await redis.GetAsync<object>(args[1]);
        Console.WriteLine(valueAsync ?? "<null>");
        break;
    case "set":
        if (args.Length != 3)
        {
            Syntax();
        }
        var success = redis.Set(args[1], JsonConvert.DeserializeObject(args[2]));
        Console.WriteLine("Update {0}.", success ? "succeeded" : "failed");
        break;
    case "set-async":
        if (args.Length != 3)
        {
            Syntax();
        }
        var successAsync = await redis.SetAsync(args[1], JsonConvert.DeserializeObject(args[2]));
        Console.WriteLine("Update {0}.", successAsync ? "succeeded" : "failed");
        break;
    case "setnx":
        if (args.Length != 3)
        {
            Syntax();
        }
        var successNx = redis.SetIfNotExists(args[1], JsonConvert.DeserializeObject(args[2]));
        Console.WriteLine("Set if not exists {0}.", successNx ? "succeeded" : "failed");
        break;
    case "setnx-async":
        if (args.Length != 3)
        {
            Syntax();
        }
        var successNxAsync = await redis.SetIfNotExistsAsync(args[1], JsonConvert.DeserializeObject(args[2]));
        Console.WriteLine("Set if not exists {0}.", successNxAsync ? "succeeded" : "failed");
        break;
    case "del":
        var recordsAffected = redis.Del(args[1..]);
        Console.WriteLine("{0} record{1} deleted.", recordsAffected, recordsAffected == 1 ? "" : "s");
        break;
    case "del-async":
        var recordsAffectedAsync = await redis.DelAsync(args[1..]);
        Console.WriteLine("{0} record{1} deleted.", recordsAffectedAsync, recordsAffectedAsync == 1 ? "" : "s");
        break;
    default:
        Syntax();
        break;
}

void Syntax()
{
    Console.WriteLine("Usage: CLI <cmd>, where 'cmd' is one of:");
    Console.WriteLine("  exists <key> - determines the existence of a record");
    Console.WriteLine("  get <key> - gets the value of a record");
    Console.WriteLine("  set <key> <value> (<value>...) - sets the value of a record");
    Console.WriteLine("  setnx <key> <value> (<value>...) - sets the value of a record if it does not already exist");
    Console.WriteLine("  del <key> (<key> ...) - removes one or more records");
    Console.WriteLine("Use 'exists-async', 'get-async', 'set-async' and 'del-async' to test the async functions.");
    Environment.Exit(1);
}