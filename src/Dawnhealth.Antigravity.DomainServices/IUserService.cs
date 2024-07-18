using Microsoft.AspNetCore.Identity;

namespace Dawnhealth.Antigravity.Domain.Users;
public interface IUserService
{
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password, int activationCode);

    Task<IList<ApplicationUser>> GetUsersAsync(string role);
}
