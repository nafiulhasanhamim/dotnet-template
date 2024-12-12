# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 80

# COPY dotnet-mvc.sln dotnet-mvc.sln

# COPY dotnet-mvc.csproj dotnet-mvc.csproj

# RUN dotnet restore

# COPY . .

# RUN dotnet build -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish -c Release -o /app/publish

# Use the official .NET image as the base image
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# Use the SDK image to build the application
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src

# Copy the project file
# COPY ["dotnet-mvc.csproj", "./"]

# Restore project dependencies
# RUN dotnet restore

# Copy all source files into the image
# COPY . .

# Build the project
# RUN dotnet build -c Release -o /app/build

# Publish the application
# FROM build AS publish
# RUN dotnet publish -c Release -o /app/publish

# Use the runtime image to run the application
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "dotnet-mvc.dll"]


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS final-env
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "dotnet-mvc.dll"]
