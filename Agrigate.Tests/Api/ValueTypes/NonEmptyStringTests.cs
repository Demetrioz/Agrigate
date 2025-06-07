using Agrigate.Api.Core.ValueTypes;

namespace Agrigate.Tests.Api.ValueTypes;

public class NonEmptyStringTests
{
    [TestCaseSource(
        typeof(NonEmptyStringTestCases),
        nameof(NonEmptyStringTestCases.SuccessTestCases))]
    public void NonEmptyString_Succeeds(string value)
    {
        Assert.DoesNotThrow(() =>
        {
            var result = new NonEmptyString("Success", value);
        });
    }
    
    [TestCaseSource(
        typeof(NonEmptyStringTestCases),
        nameof(NonEmptyStringTestCases.FailureTestCases))]
    public void NonEmptyString_Throws(string value)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var result = new NonEmptyString("Failure", value);
        });
    }
}

public static class NonEmptyStringTestCases
{
    public static IEnumerable<TestCaseData> SuccessTestCases
    {
        get
        {
            yield return new TestCaseData("Test");
            yield return new TestCaseData("123");
            yield return new TestCaseData(".@%#");
        }
    }
    
    public static IEnumerable<TestCaseData> FailureTestCases
    {
        get
        {
            yield return new TestCaseData("");
            yield return new TestCaseData("   ");
            yield return new TestCaseData(null);
        }
    }
}