using System.ComponentModel.DataAnnotations;

namespace Dawnhealth.Antigravity.Domain;

public class AccelerationMeasurementRequest
{
    [Required]
    [Range(1, long.MaxValue)]
    public long Timestamp { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int X { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Y { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Z { get; set; }
}