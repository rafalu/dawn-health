using Dawnhealth.Antigravity.Domain;
using Dawnhealth.Antigravity.DomainServices.Repository;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Dawnhealth.Antigravity.DomainServices;

public class ActivationCodeService : IActivationCodeService
{
    private readonly ILogger<ActivationCodeService> _logger;
    private readonly IRandomNumberGenerator _randomNumberGenerator;
    private readonly IActivationCodeRepository _repository;
    private readonly ILookupNormalizer _lookupNormalizer;

    public ActivationCodeService(ILogger<ActivationCodeService> logger, IRandomNumberGenerator randomNumberGenerator, IActivationCodeRepository repository, ILookupNormalizer lookupNormalizer)
    {
        _logger = logger;
        _randomNumberGenerator = randomNumberGenerator;
        _repository = repository;
        _lookupNormalizer = lookupNormalizer;
    }

    /// <summary>
    /// Generates a unique 6-digit code for the user
    /// </summary>
    /// <returns>Returns the generated code</returns>
    public async Task<int> GenerateCodeAsync(int adminUserId, string email)
    {
        _logger.LogInformation("Generating activation code by admin {adminId}", adminUserId);

        // normalize the email
        var normalizedEmail = _lookupNormalizer.NormalizeEmail(email);

        // the length can be configured via appsettings
        int code = _randomNumberGenerator.Generate(6);
        var activationCode = new ActivationCode
        {
            Code = code,
            AssignedToEmail = normalizedEmail,
            IsUsed = false,
            ExpiryDate = DateTimeOffset.UtcNow.AddMinutes(1), // 1 minute, can be configured via appsettings for example
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = adminUserId
        };

        //TODO: make sure the same email address does not have multiple active activation codes

        await _repository.CreateAsync(activationCode);

        return code;
    }

    /// <summary>
    /// Verifies the code for the user
    /// <param name="code"/>The code to verify</param>
    /// <returns>Returns true if the code is valid, false otherwise</returns></returns>
    public async Task<bool> VerifyCodeAsync(int code, string email)
    {
        _logger.LogInformation("Verifying code {code}", code);

        var normalizedEmail = _lookupNormalizer.NormalizeEmail(email);

        var activationCode = await _repository.GetAsync(code, normalizedEmail);
        if (activationCode == null)
        {
            _logger.LogWarning("Activation code not found for user and code {code}", code);
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

        await _repository.SaveChangesAsync();

        return true;
    }

}
