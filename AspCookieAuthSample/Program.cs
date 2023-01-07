using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

HttpResponseMessage Ok(string message = "")
{
    return new HttpResponseMessage(HttpStatusCode.OK)
    {
        RequestMessage = new HttpRequestMessage
        {
            Content = new StringContent(message)
        }
    };
}

HttpResponseMessage Unauthorized(string message = "")
{
    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
    {
        RequestMessage = new HttpRequestMessage
        {
            Content = new StringContent(message)
        }
    };
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication("cookie").AddCookie("cookie");

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/", () => "Hello World!");

app.MapGet("/username", (HttpContext ctx) =>
{
    var claim = ctx.User.FindFirst(ClaimTypes.NameIdentifier);
    return claim == null
        ? Unauthorized()
        : Ok(claim.Value);
});

app.MapGet("/login", async (HttpContext ctx) =>
{
    // TODO: authorize user
    
    var claims = new List<Claim>
    {
        new (ClaimTypes.NameIdentifier, "some.username")
    };
    var identity = new ClaimsIdentity(claims, "cookie");
    var principal = new ClaimsPrincipal(identity);

    await ctx.SignInAsync("cookie", principal);
    return Ok();
});

app.Run();