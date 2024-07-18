
using System.Text.Json;

using Dawnhealth.Antigravity.DomainServices;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Dawnhealth.Antigravity.Domain.Users;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IActivationCodeService _activationCodeService;
    private readonly IDistributedCache _cache;

    public UserService(ILogger<UserService> logger, UserManager<ApplicationUser> userManager, IActivationCodeService activationCodeService, IDistributedCache cache)
    {
        _logger = logger;
        _userManager = userManager;
        _activationCodeService = activationCodeService;
        _cache = cache;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password, int activationCode)
    {
        var valid = await _activationCodeService.VerifyCodeAsync(activationCode, user.Email);
        if (!valid)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidActivationCode",
                Description = "Invalid activation code"
            });
        }

        IdentityResult.Failed(new IdentityError { Code = "InvalidRole", Description = "Invalid role" });
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

        await InvalidateCacheAsync("TestSubject");

        return result;

        static string ScrambleEmail(ApplicationUser user) => $"{user.Email[0]}***{user.Email[^1]}@***.com";
    }

    public async Task<IList<ApplicationUser>> GetUsersAsync(string role)
    {
        var key = "Users" + role;
        var cachedUsers = await _cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cachedUsers))
        {
            return JsonSerializer.Deserialize<IList<ApplicationUser>>(cachedUsers) ?? [];
        }

        var users = await _userManager.GetUsersInRoleAsync(role);
        await _cache.SetStringAsync(key, JsonSerializer.Serialize(users), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        });

        return users;
    }

    private async Task InvalidateCacheAsync(string role)
    {
        var key = "Users" + role;
        await _cache.RemoveAsync(key);
    }
}
