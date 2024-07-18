using Dawnhealth.Antigravity.Domain;
using Dawnhealth.Antigravity.DomainServices.Repository;

using Microsoft.EntityFrameworkCore;

namespace Downhealth.Antigravity.Infrastructure.Repository;

public class ActivationCodeRepository : IActivationCodeRepository
{
    private readonly ApplicationDbContext _context;

    public ActivationCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create a new activation code
    /// </summary>
    /// <param name="activationCode">An instance of <see cref="ActivationCode"/></param>
    public Task CreateAsync(ActivationCode activationCode)
    {
        _context.ActivationCodes.Add(activationCode);
        return _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get an activation code by user id and code
    /// </summary>
    /// <param name="code">Code to verify</param>
    /// <returns>If the activation code exists, return the activation code; otherwise, return null</returns>
    public Task<ActivationCode?> GetAsync(int code, string email) =>
        _context
            .ActivationCodes
            .FirstOrDefaultAsync(x => x.Code == code && x.AssignedToEmail == email);

    /// <summary>
    /// Save changes to the database
    /// </summary>
    public Task SaveChangesAsync() =>
        _context.SaveChangesAsync();
}
