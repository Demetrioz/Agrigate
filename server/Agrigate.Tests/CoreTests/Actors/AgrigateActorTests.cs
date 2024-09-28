using Agrigate.Core.Tests;

namespace Agrigate.Tests.CoreTests.ActorTests;

[TestFixture]
public class AgrigateActorTests : AgrigateActorTest
{
    [SetUp]
    public void Setup()
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        CreateActorSystemWithDI(serviceProvider);
    }

    [Test]
    public void AgrigateActor_Instantiates()
    {
        var actor = _Sys?.ActorOf(Props.Create(() => new AgrigateTestActor()));
        Assert.That(actor, Is.Not.Null);
    }

    [Test]
    public void ForwardTo_ReturnsMessage()
    {
        var actor = _Sys?.ActorOf(Props.Create(() => new AgrigateTestActor()));
        actor.Tell(new TestMessage());
        
        var result = ExpectMsg<TestMessage>();
        Assert.That(result, Is.Not.Null);
    }
}