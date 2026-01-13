using Microsoft.AspNetCore.Mvc;
using MoorUrlShortener.Api.Models.Api;
using MoorUrlShortener.Api.Models.ShortenedUrl;
using MoorUrlShortener.Api.Services.UrlShortenerServices;

namespace MoorUrlShortener.Api.Endpoints;

public static class UrlShortenerEndpoints
{
    public static void MapUrlShortenerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(string.Empty)
            .WithTags("URL Shortener");

        group.MapPost("/", ShortenUrlAsync)
            .WithName("CreateShortenedUrl")
            .WithSummary("Create a shortened URL")
            .WithDescription("Creates a shortened version of the provided URL")
            .Produces<BaseApiResponse<CreateShortenedUrlResponse>>(StatusCodes.Status201Created)
            .Produces<BaseApiResponse>(StatusCodes.Status400BadRequest);

        group.MapGet("/s/{shortCode}", RedirectToOriginalUrlAsync)
            .WithName("RedirectToOriginalUrl")
            .WithSummary("Redirect to original URL")
            .WithDescription("Redirects to the original URL using the short code")
            .Produces(StatusCodes.Status307TemporaryRedirect)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> ShortenUrlAsync(
        [FromBody] CreateShortenedUrlRequest request,
        [FromServices] IConfiguration configuration,
        [FromServices] IUrlShortenerService urlShortenerService,
        CancellationToken cancellationToken)
    {
        var isRequestValid = request.IsValid(out var errorMessage);
        if (!isRequestValid) return Results.BadRequest(new BaseApiResponse(IsSuccess: false, errorMessage));

        var shortenedUrl = await urlShortenerService.CreateShortenedUrlAsync(
            request.OriginalUrl,
            cancellationToken
        );

        var baseUrl = configuration.GetValue<string>("BaseUrl")?.TrimEnd('/');
        var response = new CreateShortenedUrlResponse($"{baseUrl}/{shortenedUrl.ShortenedCode}");
        return Results.Created($"/{shortenedUrl.ShortenedCode}", new BaseApiResponse<CreateShortenedUrlResponse>(IsSuccess: true, response));
    }

    private static async Task<IResult> RedirectToOriginalUrlAsync(
        [FromRoute] string shortCode,
        [FromServices] IUrlShortenerService urlShortenerService,
        CancellationToken cancellationToken)
    {
        var originalUrl = await urlShortenerService.GetOriginalUrlAsync(shortCode, cancellationToken);

        return string.IsNullOrEmpty(originalUrl)
            ? Results.NotFound(new BaseApiResponse(false, "Shortened URL not found."))
            : Results.Redirect(originalUrl);
    }
}
