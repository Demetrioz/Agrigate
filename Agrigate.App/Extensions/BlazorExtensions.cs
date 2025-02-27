namespace Agrigate.App.Extensions;

public static class BlazorExtensions
{
    public static IServiceCollection AddBlazor(this IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        return services;
    }

    public static WebApplication UseBlazor(this WebApplication app)
    {
        app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode();
        
        return app;
    }
}