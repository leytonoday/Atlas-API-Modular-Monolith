using ArchTests.Shared;
using NetArchTest.Rules;
using Atlas.Web;
using System.Reflection;

namespace ArchTests.Api;

public class ApiTests : BaseTest
{
    protected static Assembly ApiAssembly => typeof(ApiAssemblyReference).Assembly;

    /// <summary>
    /// This unit test verifies that the classes within the namespace "Atlas.Web.Modules.Users" 
    /// in the specified assembly do not have dependencies on other modules.
    /// </summary>
    [Fact]
    public void UsersApi_DoesNotHaveDependency_ToOtherModules()
    {
        // Arrange
        List<string> otherModules = [PlansNamespace];

        // Act
        var result = Types.InAssembly(ApiAssembly)
            .That()
                .ResideInNamespace("Atlas.Web.Modules.Users")
            .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        // Assert
        EnsureSuccess(result);
    }

    /// <summary>
    /// This unit test verifies that the classes within the namespace "Atlas.Web.Modules.Plans" 
    /// in the specified assembly do not have dependencies on other modules.
    /// </summary>
    [Fact]
    public void PlansApi_DoesNotHaveDependency_ToOtherModules()
    {
        // Arrange
        List<string> otherModules = [UsersNamespace];

        // Act
        var result = Types.InAssembly(ApiAssembly)
            .That()
                .ResideInNamespace("Atlas.Web.Modules.Plans")
            .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        // Assert
        EnsureSuccess(result);
    }
}
