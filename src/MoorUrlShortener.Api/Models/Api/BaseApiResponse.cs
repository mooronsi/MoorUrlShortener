namespace MoorUrlShortener.Api.Models.Api;

public record BaseApiResponse(
    bool IsSuccess,
    string? ErrorMessage = null
);

public record BaseApiResponse<T>(
    bool IsSuccess,
    T? Data,
    string? ErrorMessage = null
) : BaseApiResponse(IsSuccess, ErrorMessage) where T : class;
