using Agrigate.Core.Services.RuleService;
using Agrigate.EventService.Actors.Rules;
using Agrigate.EventService.Messages;
using NSubstitute;

namespace Agrigate.Tests.EventServiceTests;

[TestFixture]
public class RuleConfirmationActorTests : TestKit
{
    private IServiceProvider _serviceProvider;
    private IRuleService _mockRuleService;

    [SetUp]
    public void Setup()
    {
        _mockRuleService = Substitute.For<IRuleService>();
        _mockRuleService
            .GetActiveRulesForDevice(
                Arg.Any<long>(), 
                Arg.Any<CancellationToken>()
            )
            .Returns([]);

        _serviceProvider = new ServiceCollection()
            .AddTransient(provider => _mockRuleService)
            .BuildServiceProvider();
    }

    [Test]
    public void Handles_ConfirmRules_Successfully()
    {
        var actor = Sys
            .ActorOf(Props.Create(() => 
                new RuleConfirmationActor(_serviceProvider))
            );
        
        actor.Tell(new ConfirmRules(1, 1));

        ExpectMsg<ConfirmationResult>();
        _mockRuleService
            .Received(1)
            .GetActiveRulesForDevice(1, Arg.Any<CancellationToken>());
    }
}