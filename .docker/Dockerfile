FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY ["MoorUrlShortener.slnx", "./"]
COPY ["src/MoorUrlShortener.Api/MoorUrlShortener.Api.csproj", "src/MoorUrlShortener.Api/"]
RUN dotnet restore "src/MoorUrlShortener.Api/MoorUrlShortener.Api.csproj"

COPY . .
WORKDIR "/src/src/MoorUrlShortener.Api"
RUN dotnet build "MoorUrlShortener.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoorUrlShortener.Api.csproj" -c Release -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app

RUN apk add --no-cache icu-libs

RUN addgroup -g 1000 appuser && \
    adduser -D -u 1000 -G appuser appuser && \
    chown -R appuser:appuser /app

COPY --from=publish --chown=appuser:appuser /app/publish .

USER appuser

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "MoorUrlShortener.Api.dll"]
