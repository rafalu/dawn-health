using Microsoft.AspNetCore.Identity;

namespace Dawnhealth.Antigravity.Domain.Users;

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole() : base() { }

    public ApplicationRole(string name) : base(name) { }
}
