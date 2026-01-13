using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoorUrlShortener.Api.Domain.Entities.ShortenedUrl;

namespace MoorUrlShortener.Api.Database.DatabaseConfigurations;

public class ShortenedUrlConfiguration : IEntityTypeConfiguration<ShortenedUrl>
{
    public void Configure(EntityTypeBuilder<ShortenedUrl> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => ShortenedUrlId.Create(x))
            .IsRequired();

        builder.Property(x => x.OriginalUrl)
            .HasMaxLength(1024)
            .IsRequired();

        builder.HasIndex(x => x.OriginalUrl)
            .IsUnique();

        builder.Property(x => x.ShortenedCode)
            .HasMaxLength(7)
            .IsRequired();

        builder.HasIndex(x => x.ShortenedCode)
            .IsUnique();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
