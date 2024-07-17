using Microsoft.AspNetCore.Mvc;

namespace Dawnhealth.Antigravity.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ActivationCodesController : ControllerBase
{
    private readonly ILogger<ActivationCodesController> _logger;

    public ActivationCodesController(ILogger<ActivationCodesController> logger)
    {
        _logger = logger;
    }
}
