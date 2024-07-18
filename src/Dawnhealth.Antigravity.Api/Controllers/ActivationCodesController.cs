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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GenerateCodeAsync([FromBody] ActivationCodeCreateRequest request)
    {
        var adminUserId = User.GetUserId();

        var code = await _activationCodeService.GenerateCodeAsync(adminUserId, request.Email);
        return Ok(code);
    }
}
