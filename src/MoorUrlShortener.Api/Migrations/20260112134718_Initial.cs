using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoorUrlShortener.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortenedUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ShortenedCode = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortenedUrls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrls_OriginalUrl",
                table: "ShortenedUrls",
                column: "OriginalUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrls_ShortenedCode",
                table: "ShortenedUrls",
                column: "ShortenedCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortenedUrls");
        }
    }
}
