using Dawnhealth.Antigravity.Domain.Public.Enum;

namespace Dawnhealth.Antigravity.Domain.Public;

public class BulkStatusResponse
{
    /// <summary>
    /// The unique identifier for the bulk operation.
    /// </summary>
    public Guid BulkId { get; set; }

    /// <summary>
    /// The status of the job.
    /// </summary>
    public JobStatus Status { get; set; }
}

