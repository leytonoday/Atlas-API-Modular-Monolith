using ArchTests.Shared;
using Atlas.Shared.IntegrationEvents;
using Atlas.Users.Application;
using Atlas.Users.Infrastructure;
using NetArchTest.Rules;
using System.Reflection;

namespace ArchTests.Modules;

public class UsersModuleTests : BaseTest
{
    /// <summary>
    /// Test that ensures that any type inside the Users module doesn't rely directly on any type from another module
    /// </summary>
    [Fact]
    public void UsersModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        // Arrange
        List<string> otherModules = [PlansNamespace];
        List<Assembly> usersAssemblies = [
            typeof(UsersInfrastructureAssemblyReference).Assembly,
            typeof(UsersApplicationAssemblyReference).Assembly,
            typeof(UsersDomainAssemblyReference).Assembly,
        ];

        // Act
        var result = Types.InAssemblies(usersAssemblies)
            .That()
                .ImplementInterface(typeof(IIntegrationEvent)) // This is because the Users module relies on the Atlas.Users.IntegrationEvents project (this is okay and allowed)
            .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        // Assert
        EnsureSuccess(result);
    }

    /// <summary>
    /// Test that ensures the Domain layer doesn't depend on any outer layers (application and infrastructure)
    /// </summary>
    [Fact]
    public void Users_Domain_DoesNotDependOn_OuterLayers()
    {
        // Arrange
        var domainTypes = Types.InNamespace($"{UsersNamespace}.Domain");
        var outerLayers = new[]
        {
            $"{UsersNamespace}.Application",
            $"{UsersNamespace}.Infrastructure"
        };

        // Act
        var result = domainTypes
            .ShouldNot()
                .HaveDependencyOnAny(outerLayers)
            .GetResult();

        // Assert
        EnsureSuccess(result);
    }

    /// <summary>
    /// Test that ensures the Application layer doesn't depend on any outer layers (infrastructure)
    /// </summary>
    [Fact]
    public void Users_Application_DoesNotDependOn_OuterLayers()
    {
        // Arrange
        var applicationTypes = Types.InNamespace($"{UsersNamespace}.Application");
        var outerLayers = new[]
        {
            $"{UsersNamespace}.Infrastructure"
        };

        // Act
        var result = applicationTypes
            .That()
                .ImplementInterface(typeof(IIntegrationEvent))
            .ShouldNot()
                .HaveDependencyOnAny(outerLayers)
            .GetResult();

        // Assert
        EnsureSuccess(result);
    }
}
