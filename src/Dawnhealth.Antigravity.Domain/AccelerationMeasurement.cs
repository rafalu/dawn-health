using Dawnhealth.Antigravity.Domain.Users;

namespace Dawnhealth.Antigravity.Domain;

public class AccelerationMeasurement
{
    public long Id { get; set; }

    public long Timestamp { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int Z { get; set; }

    public int UserId { get; set; }

    // Audit properties
    public DateTimeOffset CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }

    // Navigation properties
    public ApplicationUser? User { get; set; }
}