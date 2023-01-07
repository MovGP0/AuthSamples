using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using ResourceServer;

namespace IntegrationTests.Factories;

public static class ResourceServerFactory
{
    public static WebHostBuilder CreateHost()
    {
        var host = new WebHostBuilder();
        host.ConfigureServices(Startup.ConfigureServices);
        host.UseEnvironment("Development");
        host.Configure((context, app) =>
        {
            Startup.Configure(app, context.HostingEnvironment);
            app.UseEndpoints(b =>
            {
                Startup.ConfigureEndpoints(b, context.HostingEnvironment);
            });
        });
        return host;
    }
}