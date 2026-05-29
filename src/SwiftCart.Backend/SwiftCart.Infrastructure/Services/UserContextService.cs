using System;


using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SwiftCart.Application.Interfaces;


namespace SwiftCart.Infrastructure.Services;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public bool TryGetUserId(out Guid userId)
    {
        userId = Guid.Empty;
        var ctx = _httpContextAccessor.HttpContext;
        var user = ctx?.User;

        var idStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(idStr))
        {
            var token = ctx?.Request?.Cookies["jwt"];
            if (string.IsNullOrEmpty(token)) return false;

            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                idStr = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }
            catch
            {
                return false;
            }
        }

        return Guid.TryParse(idStr, out userId);
    }
     

    public Task<(bool Found, Guid UserId)> GetUserIdAsync()
    {
        var ok = TryGetUserId(out var id);
        return Task.FromResult((ok, id));
    }
}

