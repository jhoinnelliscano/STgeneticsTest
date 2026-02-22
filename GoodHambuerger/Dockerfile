# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY ["GoodHamburger.API/GoodHamburger.API.csproj", "GoodHamburger.API/"]
COPY ["GoodHamburger.Core/GoodHamburger.Core.csproj", "GoodHamburger.Core/"]
COPY ["GoodHamburger.Domain/GoodHamburger.Domain.csproj", "GoodHamburger.Domain/"]
COPY ["GoodHamburger.Infrastructure/GoodHamburger.Infrastructure.csproj", "GoodHamburger.Infrastructure/"]

RUN dotnet restore "GoodHamburger.API/GoodHamburger.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/app/GoodHamburger.API"
RUN dotnet build "GoodHamburger.API.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "GoodHamburger.API.csproj" -c Release -o /app/publish

# Use the ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodHamburger.API.dll"]
