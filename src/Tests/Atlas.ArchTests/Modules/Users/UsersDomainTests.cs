using Atlas.ArchTests.Shared;

namespace Atlas.ArchTests.Modules.Users;

public class UsersDomainTests : BaseDomainTests
{
    public UsersDomainTests() : base(typeof(UsersDomainAssemblyReference).Assembly) {}
}
