using System.ComponentModel.DataAnnotations;

namespace Dawnhealth.Antigravity.Domain.Public;

public class ActivationCodeCreateRequest
{
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
}
