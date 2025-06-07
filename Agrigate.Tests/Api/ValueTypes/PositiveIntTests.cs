using Agrigate.Api.Core.ValueTypes;

namespace Agrigate.Tests.Api.ValueTypes;

public class PositiveIntTests
{
    [TestCaseSource(
        typeof(PositiveIntTestCases),
        nameof(PositiveIntTestCases.SuccessTestCases))]
    public void PositiveInt_Succeeds(int value)
    {
        Assert.DoesNotThrow(() =>
        {
            var result = new PositiveInt("Success", value);
        });
    }
    
    [TestCaseSource(
        typeof(PositiveIntTestCases),
        nameof(PositiveIntTestCases.FailureTestCases))]
    public void PositiveInt_Throws(int value)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var result = new PositiveInt("Failure", value);
        });
    }
}

public static class PositiveIntTestCases
{
    public static IEnumerable<TestCaseData> SuccessTestCases
    {
        get
        {
            yield return new TestCaseData(99);
        }
    }
    
    public static IEnumerable<TestCaseData> FailureTestCases
    {
        get
        {
            yield return new TestCaseData(-1);
            yield return new TestCaseData(0);
            yield return new TestCaseData(null);
        }
    }
}