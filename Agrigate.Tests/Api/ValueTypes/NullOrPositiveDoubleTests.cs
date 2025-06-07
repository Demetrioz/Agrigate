using Agrigate.Api.Core.ValueTypes;

namespace Agrigate.Tests.Api.ValueTypes;

public class NullOrPositiveDoubleTests
{
    [TestCaseSource(
        typeof(NullOrPositiveDoubleTestCases),
        nameof(NullOrPositiveDoubleTestCases.SuccessTestCases))]
    public void NullOrPositiveDouble_Succeeds(double? value)
    {
        Assert.DoesNotThrow(() =>
        {
            var result = new NullOrPositiveDouble("Success", value);
        });
    }
    
    [TestCaseSource(
        typeof(NullOrPositiveDoubleTestCases),
        nameof(NullOrPositiveDoubleTestCases.FailureTestCases))]
    public void NullOrPositiveDouble_Throws(double? value)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var result = new NullOrPositiveDouble("Failure", value);
        });
    }
}

public static class NullOrPositiveDoubleTestCases
{
    public static IEnumerable<TestCaseData> SuccessTestCases
    {
        get
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(24.578);
            yield return new TestCaseData(99d);
        }
    }
    
    public static IEnumerable<TestCaseData> FailureTestCases
    {
        get
        {
            yield return new TestCaseData(-1d);
            yield return new TestCaseData(-2.45);
            yield return new TestCaseData(0d);
        }
    }
}