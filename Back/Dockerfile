# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Configurar NuGet para evitar timeouts
ENV NUGET_HTTP_CACHE_PATH=/tmp/NuGetCache
RUN dotnet nuget locals all --clear

# Copy everything (mais simples e evita problemas de caminho)
COPY . .

# Restore e Build em comandos separados com configurações de timeout
WORKDIR /src/src/BancoPan.Cadastro.Api
RUN dotnet restore --disable-parallel --force --no-cache \
    --configfile /src/nuget.config || dotnet restore --disable-parallel

# Build e Publish
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Instalar curl para healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "BancoPan.Cadastro.Api.dll"]
