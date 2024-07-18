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
    private readonly IUserService _userService;

    public UsersController(ILogger<UsersController> logger, IUserService userService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager, IActivationCodeService activationCodeService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("test-subjects/sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest request)
    {
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email
        };

        try
        {
            var result = await _userService.CreateAsync(user, request.Password, request.ActivationCode);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to create user.");
            return StatusCode(500);
        }

        return Created();
    }

    [HttpPost("test-subjects/sign-in")]
    public Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
    {
        //TODO: create JWT token
        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpGet("test-subjects/email")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<string>>> GetEmailsAsync()
    {
        var emails = (await _userService.GetUsersAsync("TestSubject")).Select(u => u.Email);
        return Ok(emails);
    }
}
