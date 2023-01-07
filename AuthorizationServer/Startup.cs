namespace AuthorizationServer;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddRouting();
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseRouting();
    }

    public static void ConfigureEndpoints(IEndpointRouteBuilder app, IWebHostEnvironment env)
    {
        app.MapGet("/", () => "Hello World");
    }
}