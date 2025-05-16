FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /micro-ref
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
EXPOSE 5432
WORKDIR /micro-ref
COPY --from=build /micro-ref/out .
ENTRYPOINT ["dotnet", "MobyLabWebProgramming.Backend.dll"] 