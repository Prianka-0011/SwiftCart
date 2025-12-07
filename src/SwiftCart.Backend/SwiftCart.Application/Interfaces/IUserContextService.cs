using System;
using System.Threading.Tasks;

namespace SwiftCart.Application.Interfaces;

public interface IUserContextService
{
    /// <summary>
    /// Try to obtain the authenticated user id from the current HttpContext (claims or cookie JWT fallback).
    /// Returns true if an id was found and parsed as Guid.
    /// </summary>
    bool TryGetUserId(out Guid userId);

    /// <summary>
    /// Async variant (keeps API flexible for future async validation).
    /// </summary>
    Task<(bool Found, Guid UserId)> GetUserIdAsync();
}
