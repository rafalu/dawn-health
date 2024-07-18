using Dawnhealth.Antigravity.Domain;

namespace Dawnhealth.Antigravity.DomainServices.Repository;

public interface IAccelerationMeasurementRepository
{
    Task CreateBulkAsync(IEnumerable<AccelerationMeasurement> measurements);
}