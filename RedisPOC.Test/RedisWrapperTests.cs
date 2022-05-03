using System;
using NUnit.Framework;

namespace RedisPOC.Test;

public class RedisWrapperTests
{
    private readonly IRedisWrapper _redis = new RedisWrapper();
    
    [SetUp]
    public void SetUp()
    {
        _redis.Set("test_user_key", new ExampleObject
        {
            Id = Guid.Parse("00112233-4455-6677-8899-aabbccddeeff"),
            Name = "Test User"
        });
        _redis.Set("test_null_value", null);
    }

    [TearDown]
    public void TearDown()
    {
        _redis.Del(new[] {"test_user_key", "test_null_value", "test_set_null_value", "test_another_user"});
    }

    [Test]
    public void TestExists_False()
    {
        var result = _redis.Exists("this_will_never_exist");
        Assert.False(result);
    }
    
    [Test]
    public void TestExists_True_NullValue()
    {
        var result = _redis.Exists("test_null_value");
        Assert.True(result);
    }
    
    [Test]
    public void TestExists_True_NonNullValue()
    {
        var result = _redis.Exists("test_user_key");
        Assert.True(result);
    }
    
    [Test]
    public void TestGet_Nonexistent()
    {
        var result = _redis.Get<ExampleObject>("this_will_never_exist");
        Assert.IsNull(result);
    }
    
    [Test]
    public void TestGet_NullValue()
    {
        var result = _redis.Get<ExampleObject>("this_will_never_exist");
        Assert.IsNull(result);
    }
    
    [Test]
    public void TestGet_NonNullValue()
    {
        var nullableObject = _redis.Get<ExampleObject>("test_user_key");
        Assert.IsNotNull(nullableObject);
        var realObject = nullableObject!;
        Assert.AreEqual(Guid.Parse("00112233-4455-6677-8899-aabbccddeeff"), realObject.Id);
        Assert.AreEqual("Test User", realObject.Name);
    }
    
    [Test]
    public void TestSet_Overwrite()
    {
        var replacement = new ExampleObject
        {
            Id = Guid.Parse("ffeeddcc-bbaa-9988-7766-554433221100"),
            Name = "Replacement Object"
        };
        var result = _redis.Set("test_user_key", replacement);
        Assert.True(result);
        var nullableObject = _redis.Get<ExampleObject>("test_user_key");
        Assert.IsNotNull(nullableObject);
        var realObject = nullableObject!;
        Assert.AreEqual(Guid.Parse("ffeeddcc-bbaa-9988-7766-554433221100"), realObject.Id);
        Assert.AreEqual("Replacement Object", realObject.Name);
    }
    
    [Test]
    public void TestSet_NullValue()
    {
        var result = _redis.Set("test_set_null_value", null);
        Assert.True(result);
        var exists = _redis.Exists("test_set_null_value");
        Assert.True(exists);
        var nullableObject = _redis.Get<ExampleObject>("test_set_null_value");
        Assert.IsNull(nullableObject);
    }
    
    [Test]
    public void TestSet_NonNullValue()
    {
        var anotherUser = new ExampleObject
        {
            Id = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
            Name = "Another User"
        };
        var result = _redis.Set("test_another_user", anotherUser);
        Assert.True(result);
        var exists = _redis.Exists("test_another_user");
        Assert.True(exists);
        var nullableObject = _redis.Get<ExampleObject>("test_another_user");
        Assert.IsNotNull(nullableObject);
        var realObject = nullableObject!;
        Assert.AreEqual(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), realObject.Id);
        Assert.AreEqual("Another User", realObject.Name);
    }

    [Test]
    public void TestDel_NoRecords()
    {
        var result = _redis.Del(new[] {"this_will_never_exist"});
        Assert.AreEqual(0, result);
    }
    
    [Test]
    public void TestDel_OneRecord()
    {
        var result = _redis.Del(new[] {"test_user_key"});
        Assert.AreEqual(1, result);
    }
    
    [Test]
    public void TestDel_MultipleRecordsIncludingNonexistentOnes()
    {
        var result = _redis.Del(new[] {"test_user_key", "test_null_value", "this_will_never_exist"});
        Assert.AreEqual(2, result);
    }
}

public class ExampleObject
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
}
