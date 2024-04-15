using NetArchTest.Rules;
using FluentAssertions;

namespace Atlas.Tests.Shared;

/// <summary>
/// Provides utility methods for testing.
/// </summary>
public static class TestUtils
{
    /// <summary>
    /// Asserts that the test result is successful.
    /// </summary>
    /// <param name="testResult">The test result to assert.</param>
    public static void AssertSuccess(TestResult testResult)
    {
        testResult.IsSuccessful.Should().BeTrue();
    }

    /// <summary>
    /// Asserts that the collection of types is either null or empty.
    /// </summary>
    /// <param name="types">The collection of types to assert.</param>
    public static void AssertFailingTypes(IEnumerable<Type> types)
    {
        types.Should().BeNullOrEmpty();
    }
}