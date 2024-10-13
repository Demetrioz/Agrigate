using System.Collections;
using Agrigate.Core.Services.NotificationService;
using Agrigate.Core.Services.RuleService;
using Agrigate.Core.Services.RuleService.Models;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities;
using Agrigate.Domain.Entities.Rules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using NSubstitute;

namespace Agrigate.Tests.CoreTests.Services;

[TestFixture]
public class RuleServiceTests
{
    public static readonly string _ruleName = "TestRule";
    public static readonly string _telemetryKey = "TestKey";
    public static readonly double _telemetryValue = 25;
    public static readonly string _notificationAddress = "testTopic";
    public static readonly string _notificationContent = "Test Content!";

    private DbContextOptions<AgrigateContext> _contextOptions;
    private INotificationService _mockNotificationService;

    [SetUp]
    public void Setup()
    {
        _contextOptions = new DbContextOptionsBuilder<AgrigateContext>()
            .UseInMemoryDatabase(nameof(RuleServiceTests))
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _mockNotificationService = Substitute.For<INotificationService>();
        _mockNotificationService.SendMqttNotification(
            Arg.Any<string>(),
            Arg.Any<string>()
        ).Returns(Task.CompletedTask);
    }

    [Test, Order(1)]
    public async Task GetRulesForDevice_ReturnsEmptyWhenNotFound()
    {
        using var context = new AgrigateContext(_contextOptions);
        await AddInactiveBaseData(context);
        var ruleService = new RuleService(context, _mockNotificationService);

        var result = await ruleService.GetActiveRulesForDevice(10);

        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test, Order(2)]
    public async Task GetRulesForDevice_Succeeds()
    {
        using var context = new AgrigateContext(_contextOptions);
        await ActivateRule(context);
        var ruleService = new RuleService(context, _mockNotificationService);

        var result = await ruleService.GetActiveRulesForDevice(1);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.EqualTo(1));
    }

    [TestCaseSource(typeof(RuleServiceTestCases), nameof(RuleServiceTestCases.ValidateTelemetryRule_TestCases)), Order(3)]
    public async Task ValidateTelemetryRule_ReturnsExpectedResult(
        long telemetryId,
        long ruleId,
        int expectedActions,
        bool shouldThrow,
        bool shouldValidate
    )
    {
        using var context = new AgrigateContext(_contextOptions);
        var ruleService = new RuleService(context, _mockNotificationService);
        
        var task = ruleService.ValidateTelemetryRule(telemetryId, ruleId);

        if (shouldThrow)
            Assert.ThrowsAsync<ApplicationException>(async () => await task);

        else
        {
            var result = await task;

            Assert.Multiple(() => 
            {
                Assert.That(result.Actions, Has.Count.EqualTo(expectedActions));
                Assert.That(result.Validated, Is.EqualTo(shouldValidate));
            });
        }
    }

    [TestCaseSource(typeof(RuleServiceTestCases), nameof(RuleServiceTestCases.ExecuteTelemetryAction_TestCases)), Order(4)]
    public async Task ExecuteTelemetryAction_ReturnsExpectedResult(
        long actionId,
        List<long> telemetryIds,
        bool shouldThrow
    )
    {
        var messageTitle = $"Agrigate Rule Triggered ({_ruleName})";
        var messageText = $"{_notificationContent} - {_telemetryKey}: {_telemetryValue}";
        var messageContent = $"{messageTitle}: {messageText}";

        using var context = new AgrigateContext(_contextOptions);
        var ruleService = new RuleService(context, _mockNotificationService);

        var task = ruleService.ExecuteTelemetryAction(actionId, telemetryIds);
        
        if (shouldThrow)
            Assert.ThrowsAsync<ApplicationException>(async () => await task);

        else
        {
            await task;

            await _mockNotificationService.Received(1)
                .SendMqttNotification(
                    _notificationAddress, 
                    messageContent, 
                    Arg.Any<CancellationToken>()
                );
        }
    }

    [TestCaseSource(typeof(RuleServiceTestCases), nameof(RuleServiceTestCases.CreateDeviceRules_TestCases)), Order(5)]
    public async Task CreateDeviceRules_ReturnsExpectedResult(
        string testCaseName,
        long deviceId,
        string ruleName,
        int timespan,
        List<CreateRuleCondition> conditions,
        List<CreateRuleAction> actions,
        bool shouldThrow
    )
    {
        using var context = new AgrigateContext(_contextOptions);
        var ruleService = new RuleService(context, _mockNotificationService);
        var request = BuildRuleRequest(
            deviceId,
            ruleName,
            timespan,
            conditions,
            actions
        );

        var task = ruleService.CreateDeviceRules(request);

        if (shouldThrow)
            Assert.ThrowsAsync<ApplicationException>(async () => await task);

        else
        {
            var result = await task;

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(1));
                Assert.That(result[0].DeviceId, Is.EqualTo(deviceId));
                Assert.That(result[0].Name, Is.EqualTo(ruleName));
                Assert.That(result[0].Actions!, Has.Count.EqualTo(actions.Count));
                Assert.That(result[0].Conditions!, Has.Count.EqualTo(conditions.Count));
            });
        }
    }

    [Test, Order(6)]
    public async Task GetRuleConditionDefinitions_Succeeds()
    {
        using var context = new AgrigateContext(_contextOptions);
        await AddRuleDefinitions(context);
        var ruleService = new RuleService(context, _mockNotificationService);

        var result = await ruleService.GetRuleConditionDefinitions();

        Assert.Multiple(() => 
        {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Type, Is.EqualTo(RuleCondition.UpperLimit));
        });
    }

    [Test, Order(7)]
    public async Task GetRuleActionDefinitions_Succeeds()
    {
        using var context = new AgrigateContext(_contextOptions);
        var ruleService = new RuleService(context, _mockNotificationService);

        var result = await ruleService.GetRuleActionDefinitions();

        Assert.Multiple(() => 
        {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Type, Is.EqualTo(RuleAction.Notification));
        });
    }

    private async Task AddInactiveBaseData(AgrigateContext context)
    {
        var device = new Device
        {
            Name = "TetDevice",
            Location = "TestLocation",
            Rules =
            [
                new TelemetryRule
                {
                    Name = _ruleName,
                    IsActive = false,
                    Operator = Operator.And,
                    Timespan = 0,
                    Conditions = 
                    [
                        new TelemetryRuleCondition
                        {
                            Type = RuleCondition.UpperLimit,
                            Key = _telemetryKey,
                            Definition = JsonConvert.SerializeObject(
                                new UpperLimitDefinition
                                {
                                    Value = 10
                                })
                        }
                    ],
                    Actions = 
                    [
                        new TelemetryRuleAction
                        {
                            Type = RuleAction.Notification,
                            Definition = JsonConvert.SerializeObject(
                                new NotificationDefinition
                                {
                                    Channel = NotificationChannel.MQTT,
                                    Address = _notificationAddress,
                                    Content = _notificationContent
                                })
                        }
                    ]
                }
            ],
            Telemetry = 
            [
                new Telemetry
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    Key = _telemetryKey,
                    Value = _telemetryValue
                }
            ]
        };
        context.Add(device);
        await context.SaveChangesAsync();
    }

    private async Task ActivateRule(AgrigateContext context)
    {
        var test1Rule = await context.TelemetryRules
            .FirstOrDefaultAsync(r => r.Name == _ruleName);
        test1Rule!.IsActive = true;
        await context.SaveChangesAsync();
    }

    private async Task AddRuleDefinitions(AgrigateContext context)
    {
        var testCondition = new TelemetryRuleConditionDefinition
        {
            Type = RuleCondition.UpperLimit,
        };

        var testAction = new TelemetryRuleActionDefinition
        {
            Type = RuleAction.Notification
        };

        context.Add(testCondition);
        context.Add(testAction);
        await context.SaveChangesAsync();
    }

    private DeviceRules BuildRuleRequest(
        long deviceId,
        string ruleName,
        int timespan,
        List<CreateRuleCondition> conditions,
        List<CreateRuleAction> actions
    )
    {
        return new DeviceRules
        {
            DeviceId = deviceId,
            Rules = new List<RuleDefinition>
            {
                new RuleDefinition
                {
                    Name = ruleName,
                    Operator = Operator.And,
                    Timespan = timespan,
                    IsActive = true,
                    Conditions = conditions,
                    Actions = actions
                }
            }
        };
    }
}

public class RuleServiceTestCases
{
    public static CreateRuleCondition CreateRule(
        string key, 
        RuleCondition type = RuleCondition.UpperLimit, 
        string? definition = null)
    {
        return new CreateRuleCondition
        {
            Key = key,
            Type = type,
            Definition = definition ?? JsonConvert.SerializeObject(new UpperLimitDefinition { Value = 10 })
        };
    }

    public static CreateRuleAction CreateAction(
        RuleAction type = RuleAction.Notification,
        string? definition = null
    )
    {
        return new CreateRuleAction
        {
            Type = type,
            Definition = definition ?? JsonConvert.SerializeObject(new NotificationDefinition
            {
                Channel = NotificationChannel.MQTT,
                Address = RuleServiceTests._notificationAddress,
                Content = RuleServiceTests._notificationContent
            })
        };
    }

    public static IEnumerable ValidateTelemetryRule_TestCases
    {
        /*
        long telemetryId,
        long ruleId,
        int expectedActions,
        bool shouldThrow,
        bool shouldValidate
        */
        get
        {
            // Telemetry is incorrect
            yield return new TestCaseData(999, 1, 0, true, false);
            // Rule is incorrect
            yield return new TestCaseData(1, 999, 0, true, false);
            // Both values are incorrect
            yield return new TestCaseData(999, 999, 0, true, false);
            // Both values are correct
            yield return new TestCaseData(1, 1, 1, false, true);
        }
    }

    public static IEnumerable ExecuteTelemetryAction_TestCases
    {
        /*
        long actionId,
        List<long> telemetryIds,
        bool shouldThrow,
        */
        get
        {
            // Action is incorrect
            yield return new TestCaseData(999, new List<long> { 1 }, true);
            // Telemetry is incorrect
            yield return new TestCaseData(1, new List<long> { 999 }, true);
            // Both values are incorrect
            yield return new TestCaseData(999, new List<long> { 999 }, true);
            // Both values are correct
            yield return new TestCaseData(1, new List<long> { 1 }, false);
        }
    }

    public static IEnumerable CreateDeviceRules_TestCases
    {
        /*
        string testCaseName,
        long deviceId,
        string ruleName,
        int timespan,
        List<CreateRuleCondition> conditions,
        List<CreateRuleAction> actions,
        bool shouldThrow
        */
        get
        {
            yield return new TestCaseData(
                "Incorrect deviceId",
                999,
                "NewRule",
                0,
                new List<CreateRuleCondition>
                {
                    CreateRule(RuleServiceTests._telemetryKey)
                },
                new List<CreateRuleAction>
                {
                    CreateAction()
                },
                true
            );
            yield return new TestCaseData(
                "Incorrect timespan",
                1,
                "NewRule",
                -5,
                new List<CreateRuleCondition>
                {
                    CreateRule(RuleServiceTests._telemetryKey)
                },
                new List<CreateRuleAction>
                {
                    CreateAction()
                },
                true
            );
            yield return new TestCaseData(
                "Incorrect condition count",
                1,
                "NewRule",
                0,
                new List<CreateRuleCondition>
                {
                },
                new List<CreateRuleAction>
                {
                    CreateAction()
                },
                true
            );
            yield return new TestCaseData(
                "Incorrect action count",
                1,
                "NewRule",
                0,
                new List<CreateRuleCondition>
                {
                    CreateRule(RuleServiceTests._telemetryKey)
                },
                new List<CreateRuleAction>
                {
                },
                true
            );
            yield return new TestCaseData(
                "Duplicate rule name",
                1,
                RuleServiceTests._ruleName,
                0,
                new List<CreateRuleCondition>
                {
                    CreateRule(RuleServiceTests._telemetryKey)
                },
                new List<CreateRuleAction>
                {
                    CreateAction()
                },
                true
            );
            yield return new TestCaseData(
                "Incorrect action definition",
                1,
                "NewRule",
                0,
                new List<CreateRuleCondition>
                {
                    CreateRule(RuleServiceTests._telemetryKey)
                },
                new List<CreateRuleAction>
                {
                    CreateAction(definition: "{\"incorrect\": 1}")
                },
                true
            );
            yield return new TestCaseData(
                "Incorrect condition definition",
                1,
                "NewRule",
                0,
                new List<CreateRuleCondition>
                {
                    CreateRule(
                        RuleServiceTests._telemetryKey, 
                        definition: "{\"incorrect\": 1}"
                    )
                },
                new List<CreateRuleAction>
                {
                    CreateAction()
                },
                true
            );
            yield return new TestCaseData(
                "Correct data",
                1,
                "NewRule",
                0,
                new List<CreateRuleCondition>
                {
                    CreateRule(RuleServiceTests._telemetryKey)
                },
                new List<CreateRuleAction>
                {
                    CreateAction()
                },
                false
            );
        }
    }
}