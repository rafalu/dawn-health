using Dawnhealth.Antigravity.Domain;
using Dawnhealth.Antigravity.Domain.Extensions;
using Dawnhealth.Antigravity.Domain.Public;
using Dawnhealth.Antigravity.DomainServices;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawnhealth.Antigravity.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ActivationCodesController : ControllerBase
{
    private readonly ILogger<ActivationCodesController> _logger;
    private readonly IActivationCodeService _activationCodeService;

    public ActivationCodesController(ILogger<ActivationCodesController> logger, IActivationCodeService activationCodeService)
    {
        _logger = logger;
        _activationCodeService = activationCodeService;
    }

    [HttpPost]
    public async Task<IActionResult> GenerateCodeAsync([FromBody] ActivationCodeCreateRequest request)
    {
        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }
        var adminUserId = User.GetUserId();
        if (adminUserId == null)
        {
            return Unauthorized();
        }

        //TODO: Create a generic exception handler with a user-friendly message

        var code = await _activationCodeService.GenerateCodeAsync(adminUserId.Value, request.UserId);
        return Ok(code);
    }

    [HttpPost]
    public async Task<ActionResult<ActivationCode>> VerifyAsync([FromRoute] int code)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var success = await _activationCodeService.VerifyCodeAsync(userId.Value, code);

        return Ok(new { success });
    }
}
