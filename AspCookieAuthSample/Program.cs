using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Shared;
using static Shared.HttpHelper;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(AuthScheme.Cookie).AddCookie("cookie");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("mypolicy", config =>
    {
        config.RequireAuthenticatedUser()
            .AddAuthenticationSchemes(AuthScheme.Cookie)
            .RequireClaim(ClaimTypes.NameIdentifier, "some.username");
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/username", (HttpContext ctx) =>
{
    var claim = ctx.User.FindFirst(ClaimTypes.NameIdentifier);
    return claim == null
        ? Unauthorized()
        : Ok(claim.Value);
});

app.Map("/secret", () => "some secret")
    .RequireAuthorization("mypolicy");

app.MapGet("/login", async (HttpContext ctx) =>
{
    // TODO: authorize user
    
    var claims = new List<Claim>
    {
        new (ClaimTypes.NameIdentifier, "some.username")
    };
    var identity = new ClaimsIdentity(claims, AuthScheme.Cookie);
    var principal = new ClaimsPrincipal(identity);

    await ctx.SignInAsync(AuthScheme.Cookie, principal);
    return Ok();
});

app.Run();