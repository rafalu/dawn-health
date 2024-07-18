namespace Dawnhealth.Antigravity.DomainServices;

public interface IActivationCodeService
{
    public Task<int> GenerateCodeAsync(int adminUserId, string email);

    public Task<bool> VerifyCodeAsync(int code, string email);
}
