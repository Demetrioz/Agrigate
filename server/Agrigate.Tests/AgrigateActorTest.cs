namespace Agrigate.Tests;

/// <summary>
/// Base test class for actors that implements an ActorSystem with Dependency
/// injection
/// </summary>
public class AgrigateActorTest : TestKit
{
    protected ActorSystem? _Sys;

    /// <summary>
    /// Creates our own actor system with a Dependency resolver. TODO: Figure
    /// out how to add a DependencyResolverSetup to Sys instead of re-creating
    /// our own
    /// </summary>
    protected void CreateActorSystemWithDI(IServiceProvider serviceProvider)
    {
        var bootstrap = BootstrapSetup.Create();
        var di = DependencyResolverSetup.Create(serviceProvider);
        var actorSystemSetup = bootstrap.And(di);
        _Sys = ActorSystem.Create("TestSystem", actorSystemSetup);
    }
}