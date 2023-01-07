using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;

namespace CookieAuthServer;

public sealed class AuthService
{
    public AuthService(
        IDataProtectionProvider dataProtectionProvider,
        IHttpContextAccessor accessor)
    {
        DataProtectionProvider = dataProtectionProvider;
        Accessor = accessor;
    }

    private IDataProtectionProvider DataProtectionProvider { get; }
    private IHttpContextAccessor Accessor { get; }

    public void SignIn()
    {
        var protector = DataProtectionProvider.CreateProtector("auth-cookie");
        var key = ClaimTypes.NameIdentifier;
        var value = "some.username";
        var authSecret = protector.Protect($"{key}:{value}");
        var cookie = $"auth={authSecret}";
        Accessor.HttpContext?.Response.Cookies.Append("cookie", cookie);
    }
}