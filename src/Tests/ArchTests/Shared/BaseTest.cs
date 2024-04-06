using NetArchTest.Rules;
using FluentAssertions;

namespace ArchTests.Shared;

public abstract class BaseTest
{
    protected const string PlansNamespace = "Atlas.Plans";
    protected const string UsersNamespace = "Atlas.Users";

    protected static void EnsureSuccess(TestResult testResult)
    {
        testResult.IsSuccessful.Should().BeTrue();
    }
}
