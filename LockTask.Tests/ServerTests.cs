using LockTask.Core;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class ServerTests
{
    [SetUp]
    public void SetUp()
    {
        var countField = typeof(Server)
            .GetField("_count", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        
        countField.SetValue(null, 0);
    }
    
    [Test]
    public void Reader_ShouldReturnInitialZero()
    {
        var result = Server.GetCount();
        
        Assert.That(result, Is.EqualTo(0));
    }
    
    [Test]
    public void Writer_ShouldIncreaseValue()
    {
        const int valueToIncrease = 5;
        
        Server.AddToCount(valueToIncrease);
        var countOnServer = Server.GetCount();
        
        Assert.That(countOnServer, Is.EqualTo(valueToIncrease));
    }
    
    [Test]
    public async Task Writers_ShouldIncreaseValue()
    {
        const int writersCount = 1000;

        var tasks = Enumerable.Range(0, writersCount)
            .Select(_ => Task.Run(() => Server.AddToCount(1)));
        
        await Task.WhenAll(tasks);

        var countOnServer = Server.GetCount();

        Assert.That(countOnServer, Is.EqualTo(writersCount));
    }
    
    [Test]
    public void Reader_GetCountWhileWritingDoesNotThrow()
    {
        Parallel.For(0, 1000, i =>
        {
            if (i % 2 == 0)
            {
                Server.AddToCount(1);
            }
            else
            {
                Server.GetCount();
            }
        });

        Assert.Pass();
    }
}