using ArchTests.Shared;
using Atlas.Plans.Application;
using Atlas.Plans.Domain;
using Atlas.Plans.Infrastructure;
using System.Reflection;
using Atlas.ArchTests.Shared;

namespace Atlas.ArchTests.Modules.Plans;

public class PlansModuleTests : BaseModuleTests
{
    /// <summary>
    /// <inheritdoc cref="BaseModuleTests.Module_DoesNotHave_Dependency_On_Other_Modules"/>
    /// </summary>
    [Fact]
    public void PlansModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        // Arrange
        List<string> otherModules = [Namespaces.UsersNamespace, Namespaces.LawNamespace];
        List<Assembly> plansAssemblies = [
            typeof(PlansInfrastructureAssemblyReference).Assembly,
            typeof(PlansApplicationAssemblyReference).Assembly,
            typeof(PlansDomainAssemblyReference).Assembly,
        ];

        Module_DoesNotHave_Dependency_On_Other_Modules(otherModules, plansAssemblies);
    }

    /// <summary>
    /// <inheritdoc cref="BaseModuleTests.Domain_DoesNotDependOn_OuterLayers"/>
    /// </summary>
    [Fact]
    public void Plans_Domain_DoesNotDependOn_OuterLayers()
    {
        Domain_DoesNotDependOn_OuterLayers(Namespaces.PlansNamespace);
    }

    /// <summary>
    /// <inheritdoc cref="BaseModuleTests.Application_DoesNotDependOn_OuterLayers"/>
    /// </summary>
    [Fact]
    public void Plans_Application_DoesNotDependOn_OuterLayers()
    {
        Application_DoesNotDependOn_OuterLayers(Namespaces.PlansNamespace);
    }
}

