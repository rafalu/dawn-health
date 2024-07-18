namespace Dawnhealth.Antigravity.Domain.Users;
public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetUsersAsync(string role);
}
