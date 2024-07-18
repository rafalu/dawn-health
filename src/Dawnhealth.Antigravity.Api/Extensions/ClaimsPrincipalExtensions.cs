﻿using System.Security.Claims;

namespace Dawnhealth.Antigravity.Domain.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(value, out var userId))
            return userId;
        return null;
    }
}
