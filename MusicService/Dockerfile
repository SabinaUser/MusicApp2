#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MusicService/MusicService.csproj", "MusicService/"]
COPY ["Music.DataAccess/Music.DataAccess.csproj", "Music.DataAccess/"]
COPY ["Music.Core/Music.Core.csproj", "Music.Core/"]
COPY ["Music.Shared/Music.Shared.csproj", "Music.Shared/"]
RUN dotnet restore "./MusicService/MusicService.csproj"
COPY . .
WORKDIR "/src/MusicService"
RUN dotnet build "./MusicService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MusicService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER root
RUN mkdir -p /app/wwwroot/uploads && chmod -R 777 /app/wwwroot/uploads

USER app

ENTRYPOINT ["dotnet", "MusicService.dll"]