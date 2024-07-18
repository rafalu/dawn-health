using System.Collections.Concurrent;

using Dawnhealth.Antigravity.Domain;
using Dawnhealth.Antigravity.Domain.Extensions;
using Dawnhealth.Antigravity.Domain.Public;
using Dawnhealth.Antigravity.Domain.Public.Enum;
using Dawnhealth.Antigravity.DomainServices;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawnhealth.Antigravity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccelerationMeasurementController : ControllerBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private static ConcurrentDictionary<Guid, BulkStatusResponse> _jobStatus = new();

    public AccelerationMeasurementController(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CreateBulkAsync([FromBody] List<AccelerationMeasurement> measurements, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
    {
        //TODO: Implement idempotency, so that the same request can be sent multiple times without causing side effects
        //TODO: Consider using msgpack or protobuf for serialization
        //TODO: Make sure compression is enabled

        var userId = User.GetUserId();
        var bulkId = Guid.NewGuid();

        //TODO: off-load the work to a background job (ideally, this should be done in a separate service)
        _jobStatus[bulkId] = new BulkStatusResponse
        {
            BulkId = bulkId,
            Status = JobStatus.Pending
        };

        _ = Task.Run(async () =>
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IAccelerationMeasurementService>();
            await service.CreateBulkAsync(measurements, userId);
            _jobStatus[bulkId].Status = JobStatus.Completed;
        });

        // return the status as soon as possible, not to block the client
        Request.Headers.Location = Url.Action("GetBulkCreateStatus", new { id = bulkId });
        return Accepted();
    }


    [HttpGet("status/{id}")]
    public IActionResult GetBulkCreateStatus(Guid id)
    {
        if (_jobStatus.TryGetValue(id, out var status))
            return Ok(status);

        return NotFound();
    }
}
