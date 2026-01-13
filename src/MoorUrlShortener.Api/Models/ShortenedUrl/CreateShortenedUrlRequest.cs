namespace MoorUrlShortener.Api.Models.ShortenedUrl;

public record CreateShortenedUrlRequest(string OriginalUrl)
{
    public bool IsValid(out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(OriginalUrl))
        {
            errorMessage = "Original URL cannot be empty.";
            return false;
        }

        if (!Uri.TryCreate(OriginalUrl, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        {
            errorMessage = "Original URL must be a valid HTTP or HTTPS URL.";
            return false;
        }

        if (OriginalUrl.Length > 1024)
        {
            errorMessage = "Original URL cannot exceed 1024 characters.";
            return false;
        }

        errorMessage = null;
        return true;
    }
}
