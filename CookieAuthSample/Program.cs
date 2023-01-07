using System.Net;
using System.Security.Claims;
using CookieAuthServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCookieAuthMiddleware();

var app = builder.Build();
app.UseCookieAuthMiddleware();

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