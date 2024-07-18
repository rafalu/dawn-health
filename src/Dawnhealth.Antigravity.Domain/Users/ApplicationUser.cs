using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Dawnhealth.Antigravity.Domain.Users;

public class ApplicationUser : IdentityUser<int>
{
    [MaxLength(100)]
    [ProtectedPersonalData]
    public required string FirstName { get; set; }

    [MaxLength(100)]
    [ProtectedPersonalData]
    public required string LastName { get; set; }

    // Navigation property
    public ICollection<ActivationCode> ActivationCodes { get; set; } = [];
}
