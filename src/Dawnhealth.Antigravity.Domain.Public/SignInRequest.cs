using System.ComponentModel.DataAnnotations;

namespace Dawnhealth.Antigravity.Domain.Public;

public class SignInRequest
{
    [MaxLength(256)]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    public required string Password { get; set; }
}
