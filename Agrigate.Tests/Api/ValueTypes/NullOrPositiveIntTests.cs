using Agrigate.Api.Core.ValueTypes;

namespace Agrigate.Tests.Api.ValueTypes;

public class NullOrPositiveIntTests
{
    [TestCaseSource(
        typeof(NullOrPositiveIntTestCases),
        nameof(NullOrPositiveIntTestCases.SuccessTestCases))]
    public void NullOrPositiveInt_Succeeds(int? value)
    {
        Assert.DoesNotThrow(() =>
        {
            var result = new NullOrPositiveInt("Success", value);
        });
    }
    
    [TestCaseSource(
        typeof(NullOrPositiveIntTestCases),
        nameof(NullOrPositiveIntTestCases.FailureTestCases))]
    public void NullOrPositiveInt_Throws(int? value)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var result = new NullOrPositiveInt("Failure", value);
        });
    }
}

public static class NullOrPositiveIntTestCases
{
    public static IEnumerable<TestCaseData> SuccessTestCases
    {
        get
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(99);
        }
    }
    
    public static IEnumerable<TestCaseData> FailureTestCases
    {
        get
        {
            yield return new TestCaseData(-1);
            yield return new TestCaseData(0);
        }
    }
}