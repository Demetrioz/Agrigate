using Agrigate.Core.Services.RuleService;
using Agrigate.EventService.Actors.Rules;
using Agrigate.EventService.Messages;
using NSubstitute;

namespace Agrigate.Tests.EventServiceTests;

[TestFixture]
public class RuleActionActorTests : TestKit
{
    private IServiceProvider _serviceProvider;
    private IRuleService _mockRuleService;

    [SetUp]
    public void Setup()
    {
        _mockRuleService = Substitute.For<IRuleService>();
        _mockRuleService
            .ExecuteTelemetryAction(Arg.Any<long>(), Arg.Any<List<long>>())
            .Returns(Task.CompletedTask);

        _serviceProvider = new ServiceCollection()
            .AddTransient(provider => _mockRuleService)
            .BuildServiceProvider();
    }

    [Test]
    public void Handles_InitiateAction_Successfully()
    {
        var actor = Sys
            .ActorOf(Props.Create(() => 
                new RuleActionActor(_serviceProvider))
            );

        var actionId = 1;
        var telemtryIds = new List<long> { 1 };
        actor.Tell(new InitiateAction(actionId, telemtryIds));

        ExpectMsg<ActionResult>();
        _mockRuleService
            .Received(1)
            .ExecuteTelemetryAction(
                actionId, 
                telemtryIds, 
                Arg.Any<CancellationToken>()
            );
    }
}