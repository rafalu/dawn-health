
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Dawnhealth.Antigravity.Domain.Users;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMemoryCache _cache;

    public UserService(ILogger<UserService> logger, UserManager<ApplicationUser> userManager, IMemoryCache cache)
    {
        _logger = logger;
        _userManager = userManager;
        _cache = cache;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return result;
        }

        //assign user TestSubject role
        var newUser = await _userManager.FindByEmailAsync(user.Email);
        if (newUser == null) //this should never happen
        {
            throw new InvalidOperationException($"User {ScrambleEmail(user)} not found after creation");
        }

        result = await _userManager.AddToRoleAsync(user, "TestSubject");
        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to assign role to user {user}", user.Id);
        }

        InvalidateCache("TestSubject");

        return result;

        static string ScrambleEmail(ApplicationUser user) => $"{user.Email[0]}***{user.Email[^1]}@***.com";
    }

    public async Task<IList<ApplicationUser>> GetUsersAsync(string role)
    {
        // Check if users are already in cache
        var key = "Users" + role;
        if (_cache.TryGetValue(key, out IList<ApplicationUser>? users) && users != null)
        {
            return users;
        }

        users = await _userManager.GetUsersInRoleAsync(role);
        _cache.Set(key, users);

        return users;
    }

    private void InvalidateCache(string role)
    {
        _cache.Remove("Users" + role);
    }
}
