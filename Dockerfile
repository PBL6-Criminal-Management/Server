# Use the official ASP.NET runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .

# Build the application
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

# Copy the EmguCV XML file needed for face detection
COPY ["Infrastructure/Services/FaceDetect/haarcascade_frontalface_default.xml", "/app/publish/Infrastructure/Services/FaceDetect/"]

# Publish the application
FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official ASP.NET runtime image as the final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app

# Copy the published application and the EmguCV XML file
COPY --from=publish /app/publish .
COPY --from=publish /app/publish/Infrastructure/Services/FaceDetect/haarcascade_frontalface_default.xml /app/publish/Infrastructure/Services/FaceDetect/

# Set the working directory and specify the entry point
WORKDIR /app
ENTRYPOINT ["dotnet", "WebApi.dll"]
