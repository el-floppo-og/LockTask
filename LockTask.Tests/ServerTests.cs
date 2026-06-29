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
        
        Assert.That(0, Is.EqualTo(result));
    }
    
    [Test]
    public void Writer_ShouldIncreaseValue()
    {
        const int valueToIncrease = 5;
        
        Server.AddToCount(valueToIncrease);
        var countOnServer = Server.GetCount();
        
        Assert.That(valueToIncrease, Is.EqualTo(countOnServer));
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
}