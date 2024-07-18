using Dawnhealth.Antigravity.Domain;
using Dawnhealth.Antigravity.DomainServices.Repository;

namespace Dawnhealth.Antigravity.DomainServices;
public class AccelerationMeasurementService : IAccelerationMeasurementService
{
    private readonly IAccelerationMeasurementRepository _repository;

    public AccelerationMeasurementService(IAccelerationMeasurementRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateBulkAsync(IEnumerable<AccelerationMeasurement> measurements, int userId)
    {
        foreach (var measurement in measurements)
        {
            measurement.UserId = userId;
        }

        await _repository.CreateBulkAsync(measurements);
    }
}
