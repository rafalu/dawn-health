using Dawnhealth.Antigravity.Domain;

namespace Dawnhealth.Antigravity.DomainServices.Repository;

public interface IActivationCodeRepository
{
    Task CreateAsync(ActivationCode activationCode);

    Task<ActivationCode?> GetAsync(int code, string email);

    Task SaveChangesAsync();
}
