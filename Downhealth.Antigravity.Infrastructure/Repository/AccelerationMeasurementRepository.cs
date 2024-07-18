using Dawnhealth.Antigravity.Domain;
using Dawnhealth.Antigravity.DomainServices.Repository;

namespace Downhealth.Antigravity.Infrastructure.Repository;
public class AccelerationMeasurementRepository : IAccelerationMeasurementRepository
{
    private readonly ApplicationDbContext _context;

    public AccelerationMeasurementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateBulkAsync(IEnumerable<AccelerationMeasurement> measurements)
    {
        //this will obviously be very slow and has to be optimized by using bulk insert (there are nugets available that support that)
        //as it's not defined where the data should be stored,
        //it could also be stored in Azure Table or Azure Blob Storage or any other non-relational storage
        await _context.AccelerationMeasurements.AddRangeAsync(measurements);
        await _context.SaveChangesAsync();
    }
}
