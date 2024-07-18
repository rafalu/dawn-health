using Dawnhealth.Antigravity.Domain;
using Dawnhealth.Antigravity.DomainServices.Repository;

using Microsoft.Extensions.Logging;

namespace Dawnhealth.Antigravity.DomainServices;

public class ActivationCodeService : IActivationCodeService
{
    private readonly ILogger<ActivationCodeService> _logger;
    private readonly IRandomNumberGenerator _randomNumberGenerator;
    private readonly IActivationCodeRepository _repository;

    public ActivationCodeService(ILogger<ActivationCodeService> logger, IRandomNumberGenerator randomNumberGenerator, IActivationCodeRepository repository)
    {
        _logger = logger;
        _randomNumberGenerator = randomNumberGenerator;
        _repository = repository;
    }

    /// <summary>
    /// Generates a unique 6-digit code for the user
    /// </summary>
    /// <returns>Returns the generated code</returns>
    public async Task<int> GenerateCodeAsync(int adminUserId, int activationCodeUserId)
    {
        _logger.LogInformation("Generating activation code for user {userId} by admin {adminId}", activationCodeUserId, adminUserId);

        // the length can be configured via appsettings
        int code = _randomNumberGenerator.Generate(6);
        var activationCode = new ActivationCode
        {
            Code = code,
            IsUsed = false,
            UserId = activationCodeUserId,
            ExpiryDate = DateTimeOffset.UtcNow.AddMinutes(1), // 1 minute, can be configured via appsettings for example
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = adminUserId
        };

        await _repository.CreateAsync(activationCode);

        return code;
    }

    /// <summary>
    /// Verifies the code for the user
    /// <param name="code"/>The code to verify</param>
    /// <param name="userId">User ID</param>
    /// <returns>Returns true if the code is valid, false otherwise</returns></returns>
    public async Task<bool> VerifyCodeAsync(int userId, int code)
    {
        _logger.LogInformation("Verifying code {code} for user {userId}", code, userId);

        var activationCode = await _repository.GetAsync(userId, code);
        if (activationCode == null)
        {
            _logger.LogWarning("Activation code not found for user {userId} and code {code}", userId, code);
            return false;
        }

        if (activationCode.IsUsed)
        {
            _logger.LogWarning("Activation code {code} has already been used", code);
            return false;
        }

        if (activationCode.ExpiryDate < DateTimeOffset.UtcNow)
        {
            _logger.LogWarning("Activation code {code} has expired", code);
            return false;
        }

        activationCode.IsUsed = true;
        activationCode.ModifiedAt = DateTimeOffset.UtcNow;
        activationCode.ModifiedBy = userId;

        await _repository.SaveChangesAsync();

        return true;
    }

}
