namespace Dawnhealth.Antigravity.Domain;

public class ActivationCode
{
    public int Id { get; set; }

    public int Code { get; set; }

    public bool IsUsed { get; set; }

    public required string AssignedToEmail { get; set; }

    public DateTimeOffset ExpiryDate { get; set; }

    // Audit properties
    public DateTimeOffset CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }
}