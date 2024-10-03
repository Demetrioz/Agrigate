using Agrigate.Core.Services.RuleService;
using Agrigate.Core.Services.RuleService.Models;
using Agrigate.EventService.Actors.Rules;
using Agrigate.EventService.Messages;
using NSubstitute;

namespace Agrigate.Tests.EventServiceTests;

[TestFixture]
public class RuleValidationActorTests : TestKit
{
    private IServiceProvider _serviceProvider;
    private IRuleService _mockRuleService;

    [SetUp]
    public void Setup()
    {
        _mockRuleService = Substitute.For<IRuleService>();
        _mockRuleService.ValidateTelemetryRule(Arg.Any<long>(), Arg.Any<long>())
            .Returns(Task.FromResult(new RuleValidation()));

        _serviceProvider = new ServiceCollection()
            .AddTransient(provider => _mockRuleService)
            .BuildServiceProvider();
    }

    [Test]
    public void Handles_ValidateRule_Successfully()
    {
        var actor = Sys
            .ActorOf(Props.Create(() => 
                new RuleValidationActor(_serviceProvider))
            );

        actor.Tell(new ValidateRule(1, 1));

        ExpectMsg<ValidationResult>();
        _mockRuleService
            .Received(1)
            .ValidateTelemetryRule(1, 1, Arg.Any<CancellationToken>());
    }
}