using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;

namespace Agrigate.Core.Actors;

/// <summary>
/// A base actor for the Agrigate platform that contains common functionality
/// </summary>
public abstract class AgrigateActor : ReceiveActor
{
    protected readonly ILoggingAdapter Logger;

    public AgrigateActor()
    {
        Logger = Logging.GetLogger(Context) ?? throw new ApplicationException("Unable to retrieve logger");
    }

    /// <summary>
    /// Forwards a message to a specified actor type, TActor. TActor should be 
    /// short lived and terminate itself after handling the messsage.
    /// </summary>
    /// <typeparam name="TMessage">A class representing the message that's being
    /// sent</typeparam>
    /// <typeparam name="TActor">A class representing the type of actor that 
    /// should handle the message being sent</typeparam>
    /// <param name="message">The message that needs to be handled</param>
    protected void ForwardTo<TMessage, TActor>(TMessage message)
        where TMessage : class
        where TActor : ActorBase
    {
        var queryProps = DependencyResolver.For(Context.System).Props<TActor>();
        var queryHandler = Context.ActorOf(queryProps);

        queryHandler.Forward(message);
    }
}