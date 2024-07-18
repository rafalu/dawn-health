using System.ComponentModel.DataAnnotations;

namespace Dawnhealth.Antigravity.Domain.Public;

public class UserSignUpRequest
{
    [Required]
    [DataType(DataType.Text)]
    [StringLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [StringLength(100)]
    public required string LastName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    public int ActivationCode { get; set; }
}
