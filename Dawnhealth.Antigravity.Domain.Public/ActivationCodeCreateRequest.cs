using System.ComponentModel.DataAnnotations;

namespace Dawnhealth.Antigravity.Domain.Public;

public class ActivationCodeCreateRequest
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
}
