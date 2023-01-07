using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
using Shared;

namespace CookieAuthServer;

public static class CookieAuthMiddlewareExtensions
{
    public static void AddCookieAuthMiddleware(this IServiceCollection services)
    {
        services.AddDataProtection();
        services.AddScoped<AuthService>();
    }

    private static Task CookieAuthMiddleware(HttpContext ctx, Func<Task> next)
    {
        var dpp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();

        var protector = dpp.CreateProtector("auth-cookie");

        var authCookie = ctx.Request.Headers.Cookie
            .FirstOrDefault(c => c != null && c.StartsWith("auth="));

        if (authCookie == null)
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }

        var protectedPayload = authCookie.Split("=").Last();

        var payload = protector.Unprotect(protectedPayload);
        var parts = payload.Split(":");

        var key = parts[0];
        var value = parts[1];

        var claims = new List<Claim>
        {
            new (key, value)
        };
        ctx.User = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthScheme.Cookie));
    
        return next();
    }
    
    public static void UseCookieAuthMiddleware(this WebApplication app)
    {
        app.Use(CookieAuthMiddleware);
    }
}