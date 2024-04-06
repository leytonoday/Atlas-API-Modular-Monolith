using ArchTests.Shared;
using Atlas.Plans.Application;
using Atlas.Plans.Domain;
using Atlas.Plans.Infrastructure;
using Atlas.Shared.IntegrationEvents;
using NetArchTest.Rules;
using System.Reflection;

namespace ArchTests.Modules;

public class PlansModuleTests : BaseTest
{
    /// <summary>
    /// Test that ensures that any type inside the Plans module doesn't rely directly on any type from another module
    /// </summary>
    [Fact]
    public void PlansModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        // Arrange
        List<string> otherModules = [UsersNamespace];
        List<Assembly> plansAssemblies = [
            typeof(PlansInfrastructureAssemblyReference).Assembly,
            typeof(PlansApplicationAssemblyReference).Assembly,
            typeof(PlansDomainAssemblyReference).Assembly,
        ];

        // Act
        var result = Types.InAssemblies(plansAssemblies)
            .That()
                .ImplementInterface(typeof(IIntegrationEvent)) // This is because the Plans module relies on the Atlas.Users.IntegrationEvents project (this is okay and allowed)
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
    public void Plans_Domain_DoesNotDependOn_OuterLayers()
    {
        // Arrange
        var domainTypes = Types.InNamespace($"{PlansNamespace}.Domain");
        var outerLayers = new[]
        {
            $"{PlansNamespace}.Application",
            $"{PlansNamespace}.Infrastructure"
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
    public void Plans_Application_DoesNotDependOn_OuterLayers()
    {
        // Arrange
        var applicationTypes = Types.InNamespace($"{PlansNamespace}.Application");
        var outerLayers = new[]
        {
            $"{PlansNamespace}.Infrastructure"
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
