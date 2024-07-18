using Dawnhealth.Antigravity.Domain;

namespace Dawnhealth.Antigravity.DomainServices;
public interface IAccelerationMeasurementService
{
    Task CreateBulkAsync(IEnumerable<AccelerationMeasurement> measurements, int userId);
}
