namespace Dawnhealth.Antigravity.Domain.Public.Enum;

public enum JobStatus
{
    /// <summary>
    /// The job is pending.
    /// </summary>
    Pending,

    /// <summary>
    /// The job has been completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The job has been partially successful.
    /// </summary>
    PartiallySuccessful,

    /// <summary>
    /// The job has failed.
    /// </summary>
    Failed,
}
