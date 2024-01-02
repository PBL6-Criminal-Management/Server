#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]

RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY Infrastructure/Services/exalted-pattern-400909-3eaa10f4b2b4.json /app/Infrastructure/Services/
# COPY Application/Services/Magick.Native-Q16-arm64.dll /app/Magick.Native-Q16-arm64.dll
# COPY Application/Services/Magick.Native-Q16-x64.dll /app/Magick.Native-Q16-x64.dll
# COPY Application/Services/Magick.Native-Q16-x86.dll /app/Magick.Native-Q16-x86.dll
COPY WebApi/DigiCertGlobalRootCA.crt.pem /app/DigiCertGlobalRootCA.crt.pem
# COPY WebApi/ffmpeg/x86_64 /app/ffmpeg/x86_64
COPY WebApi/Files/Avatar /app/Files/Avatar

ENTRYPOINT ["dotnet", "WebApi.dll"]