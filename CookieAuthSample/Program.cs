using System.Security.Claims;
using CookieAuthServer;
using static Shared.HttpHelper;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCookieAuthMiddleware();

var app = builder.Build();
app.UseCookieAuthMiddleware();

app.MapGet("/", () => "Hello World!");
app.MapGet("/username", (HttpContext ctx) =>
{
    var authClaim = ctx.User
        .FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

    return authClaim == null
        ? Unauthorized()
        : Ok(authClaim.Value);
});

app.MapGet("/login", (AuthService auth) =>
{
    auth.SignIn();
    return Ok();
});

app.Run();