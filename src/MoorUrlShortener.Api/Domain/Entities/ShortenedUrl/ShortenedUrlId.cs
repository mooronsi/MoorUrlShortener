namespace MoorUrlShortener.Api.Domain.Entities.ShortenedUrl;

public sealed class ShortenedUrlId : ValueObject
{
    ShortenedUrlId(Guid value) => Value = value;

    public static ShortenedUrlId Create(Guid value) =>
        value == Guid.Empty
            ? throw new ArgumentException($"{nameof(value)} cannot be empty.", nameof(value))
            : new ShortenedUrlId(value);

    public Guid Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
