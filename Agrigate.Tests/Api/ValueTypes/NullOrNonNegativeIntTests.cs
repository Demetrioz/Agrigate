using Agrigate.Api.Core.ValueTypes;

namespace Agrigate.Tests.Api.ValueTypes;

public class NullOrNonNegativeIntTests
{
    [TestCaseSource(
        typeof(NullOrNonNegativeIntTestCases),
        nameof(NullOrNonNegativeIntTestCases.SuccessTestCases))]
    public void NullOrNonNegativeInt_Succeeds(int? value)
    {
        Assert.DoesNotThrow(() =>
        {
            var result = new NullOrNonNegativeInt("Success", value);
        });
    }
    
    [TestCaseSource(
        typeof(NullOrNonNegativeIntTestCases),
        nameof(NullOrNonNegativeIntTestCases.FailureTestCases))]
    public void NullOrNonNegativeInt_Throws(int? value)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var result = new NullOrNonNegativeInt("Failure", value);
        });
    }
}

public static class NullOrNonNegativeIntTestCases
{
    public static IEnumerable<TestCaseData> SuccessTestCases
    {
        get
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(0);
            yield return new TestCaseData(99);
        }
    }
    
    public static IEnumerable<TestCaseData> FailureTestCases
    {
        get
        {
            yield return new TestCaseData(-1);
        }
    }
}