# Use the ASP.NET Core runtime base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy csproj and restore for all projects
COPY YourRest.WebApi/YourRest.WebApi.csproj YourRest.WebApi/
COPY Infrastructure/YourRest.Infrastructure.Core/YourRest.Infrastructure.Core.csproj Infrastructure/YourRest.Infrastructure.Core/
COPY Infrastructure/YourRest.Producer.Infrastructure/YourRest.Producer.Infrastructure.csproj Infrastructure/YourRest.Producer.Infrastructure/
COPY Infrastructure/YourRest.Producer.Infrastructure.Middleware/YourRest.Producer.Infrastructure.Middleware.csproj Infrastructure/YourRest.Producer.Infrastructure.Middleware/
COPY YourRest.Application/YourRest.Application.csproj YourRest.Application/
COPY YourRest.Domain/YourRest.Domain.csproj YourRest.Domain/
RUN dotnet restore "YourRest.WebApi/YourRest.WebApi.csproj"

# Copy all source files and build
COPY . .
WORKDIR /src/YourRest.WebApi
RUN dotnet build "YourRest.WebApi.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "YourRest.WebApi.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourRest.WebApi.dll"]