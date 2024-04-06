using NetArchTest.Rules;
using FluentAssertions;

namespace Atlas.Tests.Shared;

public static class TestUtils
{
    public static void AssertSuccess(TestResult testResult)
    {
        testResult.IsSuccessful.Should().BeTrue();
    }

    public static void AssertFailingTypes(IEnumerable<Type> types)
    {
        types.Should().BeNullOrEmpty();
    }
}