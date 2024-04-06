using Atlas.Shared.IntegrationEvents;
using Atlas.Tests.Shared;
using NetArchTest.Rules;
using System.Reflection;

namespace Atlas.ArchTests.Shared;

public abstract class BaseModuleTests
{
    /// <summary>
    /// Test that ensures the Domain layer doesn't depend on any outer layers (application and infrastructure)
    /// </summary>
    protected void Domain_DoesNotDependOn_OuterLayers(string @namespace)
    {
        // Arrange
        var domainTypes = Types.InNamespace($"{@namespace}.Domain");
        var outerLayers = new[]
        {
            $"{@namespace}.Application",
            $"{@namespace}.Infrastructure"
        };

        // Act
        var result = domainTypes
            .ShouldNot()
                .HaveDependencyOnAny(outerLayers)
            .GetResult();

        // Assert
        TestUtils.AssertSuccess(result);
    }

    /// <summary>
    /// Test that ensures the Application layer doesn't depend on any outer layers (infrastructure)
    /// </summary>
    protected void Application_DoesNotDependOn_OuterLayers(string @namespace)
    {
        // Arrange
        var applicationTypes = Types.InNamespace($"{@namespace}.Application");
        var outerLayers = new[]
        {
            $"{@namespace}.Infrastructure"
        };

        // Act
        var result = applicationTypes
            .That()
                .ImplementInterface(typeof(IIntegrationEvent))
            .ShouldNot()
                .HaveDependencyOnAny(outerLayers)
            .GetResult();

        // Assert
        TestUtils.AssertSuccess(result);
    }

    /// <summary>
    /// Test that ensures that any type inside the module doesn't rely directly on any type from another module
    /// </summary>
    protected void Module_DoesNotHave_Dependency_On_Other_Modules(List<string> otherModules, List<Assembly> moduleAssemblies)
    {
        // Act
        var result = Types.InAssemblies(moduleAssemblies)
            .That()
                .ImplementInterface(typeof(IIntegrationEvent)) // This is because the modules can rely on IntegrationEvent projects from other modules
            .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        // Assert
        TestUtils.AssertSuccess(result);
    }
}
