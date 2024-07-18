using Dawnhealth.Antigravity.Domain.Public;
using Dawnhealth.Antigravity.Domain.Users;
using Dawnhealth.Antigravity.DomainServices;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dawnhealth.Antigravity.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationUser> _roleManager;
    private readonly IActivationCodeService _activationCodeService;

    public UsersController(ILogger<UsersController> logger, UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager, IActivationCodeService activationCodeService)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _activationCodeService = activationCodeService;
    }

    [HttpPost("test-subjects")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest request)
    {
        if (!await ValidateActivationCode(request))
        {
            ModelState.AddModelError(nameof(UserSignUpRequest.ActivationCode), "Invalid activation code");
            return BadRequest(ModelState);
        }

        var result = await _userManager.CreateAsync(new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email
        },
        request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        //assign user TestSubject role
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) //this should never happen
        {
            //scramble email address for logging
            string email = ScrambleEmail(request);

            _logger.LogError("User {email} not found after creation", email);

            ModelState.AddModelError(nameof(request.Email), "User not found after creation");

            return BadRequest(ModelState);
        }

        result = await _userManager.AddToRoleAsync(user, "TestSubject");
        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to assign role to user {user}", user.Id);

            return BadRequest(result.Errors);
        }

        return Created();

        static string ScrambleEmail(UserSignUpRequest request) => $"{request.Email[0]}***{request.Email[^1]}@***.com";
    }

    private Task<bool> ValidateActivationCode(UserSignUpRequest request)
    {
        return _activationCodeService.VerifyCodeAsync(request.ActivationCode, request.Email);
    }

}
