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

    //public static void AssertAreImmutable(IEnumerable<Type> types)
    //{
    //    List<Type> failingTypes = [];
    //    foreach (var type in types)
    //    {
    //        if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
    //        {
    //            failingTypes.Add(type);
    //            break;
    //        }
    //    }

    //    failingTypes.Should().BeEmpty("because all types should be immutable.");
    //}
}