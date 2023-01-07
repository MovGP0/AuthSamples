namespace IntegrationTests;

[TestFixture]
public sealed class CreateServerTests
{
    [Test]
    public async Task SetupAuthServer()
    {
        using var authorizationServer = new TestServer(AuthorizationServerFactory.CreateHost());
        var authClient = authorizationServer.CreateClient();
        var response = await authClient.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }
    
    [Test]
    public async Task SetupResServer()
    {
        using var resourceServer = new TestServer(ResourceServerFactory.CreateHost());
        var resourceClient = resourceServer.CreateClient();
        var response2 = await resourceClient.GetAsync("/");
        response2.EnsureSuccessStatusCode();
    }
}