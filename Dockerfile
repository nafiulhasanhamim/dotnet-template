# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy only the project file first (for better caching)
COPY dotnet-mvc.csproj ./
RUN dotnet restore

# Copy only required source files to prevent security risks
COPY Program.cs ./Program.cs
COPY appsettings.json ./appsettings.json
COPY appsettings.Development.json ./appsettings.Development.json

# Copy all C# source files and resources while avoiding unnecessary files
COPY Controllers/ ./Controllers
COPY Models/ ./Models
COPY Services/ ./Services
COPY DTO/ ./DTO
COPY Enums/ ./Enums
COPY Extensions/ ./Extensions
COPY Interfaces/ ./Interfaces
COPY Profiles/ ./Profiles
COPY Migrations/ ./Migrations
COPY WeatherAppModel.cs/ ./WeatherAppModel.cs
COPY data/ ./data

# Build and publish the application
RUN dotnet publish -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

# Create a non-root user for security
RUN adduser --disabled-password --gecos "" appuser \
    && chown -R appuser:appuser /App

USER appuser

# Copy the published application from the build stage
COPY --from=build-env /App/out .

# Expose correct port
EXPOSE 8000

# Set ASP.NET Core to listen on the correct port
ENV ASPNETCORE_URLS=http://+:8000

# Run the application
ENTRYPOINT ["dotnet", "dotnet-mvc.dll"]
