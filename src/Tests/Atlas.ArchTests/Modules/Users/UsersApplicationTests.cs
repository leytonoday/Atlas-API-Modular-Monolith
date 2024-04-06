using Atlas.ArchTests.Shared;
using Atlas.Users.Application;

namespace Atlas.ArchTests.Modules.Users;

public class UsersApplicationTests : BaseApplicationTests
{
    public UsersApplicationTests() : base(typeof(UsersApplicationAssemblyReference).Assembly) {}
}
