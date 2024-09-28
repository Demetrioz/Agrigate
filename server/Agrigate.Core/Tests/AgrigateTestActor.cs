using Agrigate.Core.Actors;
using Akka.Actor;

namespace Agrigate.Core.Tests;

/// <summary>
/// Implementation of AgrigateActor for testing
/// </summary>
public class AgrigateTestActor : AgrigateActor
{
    public AgrigateTestActor() : base()
    {
        Receive<TestMessage>(ForwardTestMessage);
    }

    private void ForwardTestMessage(TestMessage message)
    {
        ForwardTo<TestMessage, AgrigateTestActorChild>(message);
    }
}

/// <summary>
/// Simple child actor that returns a message that has been
/// received
/// </summary>
public class AgrigateTestActorChild : AgrigateActor
{
    public AgrigateTestActorChild() : base()
    {
        ReceiveAny(ReturnMessage);
    }

    private void ReturnMessage(object message)
    {
        Sender.Tell(message);
    }
}

/// <summary>
/// A simple test message
/// </summary>
public class TestMessage();