# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiem tot codul in container
COPY . .

# Restauram dependintele
RUN dotnet restore

# Compilam aplicatia
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiem aplicatia publicata din etapa de build
COPY --from=build /app/publish .

# Expunem portul pe care aplicatia asculta (probabil 5000 sau 80)
EXPOSE 5000

# Pornim aplicatia
ENTRYPOINT ["dotnet", "MobyLabWebProgramming.Backend.dll"]
