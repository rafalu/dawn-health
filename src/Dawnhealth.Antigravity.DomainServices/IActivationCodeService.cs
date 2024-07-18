namespace Dawnhealth.Antigravity.DomainServices;

public interface IActivationCodeService
{
    public Task<int> GenerateCodeAsync(int adminUserId, int activationCodeUserId);

    public Task<bool> VerifyCodeAsync(int userId, int code);
}
