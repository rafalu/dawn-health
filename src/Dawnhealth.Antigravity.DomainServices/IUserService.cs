using Microsoft.AspNetCore.Identity;

namespace Dawnhealth.Antigravity.Domain.Users;
public interface IUserService
{
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

    Task<IList<ApplicationUser>> GetUsersAsync(string role);
}
