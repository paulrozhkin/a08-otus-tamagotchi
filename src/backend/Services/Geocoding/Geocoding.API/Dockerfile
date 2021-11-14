#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Geocoding/Geocoding.API/Geocoding.API.csproj", "Services/Geocoding/Geocoding.API/"]
COPY ["Core/Infrastructure.Core/Infrastructure.Core.csproj", "Core/Infrastructure.Core/"]
COPY ["Core/Domain.Core/Domain.Core.csproj", "Core/Domain.Core/"]
RUN dotnet restore "Services/Geocoding/Geocoding.API/Geocoding.API.csproj"
COPY . .
WORKDIR "/src/Services/Geocoding/Geocoding.API"
RUN dotnet build "Geocoding.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Geocoding.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Geocoding.API.dll"]