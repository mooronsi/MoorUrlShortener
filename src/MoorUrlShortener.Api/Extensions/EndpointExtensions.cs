using MoorUrlShortener.Api.Endpoints;

namespace MoorUrlShortener.Api.Extensions;

public static class EndpointExtensions
{
    public static IApplicationBuilder MapApiEndpoints(this WebApplication app)
    {
        app.MapUrlShortenerEndpoints();

        return app;
    }
}
