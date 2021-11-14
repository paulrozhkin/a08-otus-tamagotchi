#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Restaurants/Restaurants.API/Restaurants.API.csproj", "Services/Restaurants/Restaurants.API/"]
COPY ["Core/Infrastructure.Core/Infrastructure.Core.csproj", "Core/Infrastructure.Core/"]
COPY ["Core/Domain.Core/Domain.Core.csproj", "Core/Domain.Core/"]
COPY ["Services/Restaurants/Restaurants.Infrastructure/Restaurants.Infrastructure.csproj", "Services/Restaurants/Restaurants.Infrastructure/"]
COPY ["Services/Restaurants/Restaurants.Domain/Restaurants.Domain.csproj", "Services/Restaurants/Restaurants.Domain/"]
RUN dotnet restore "Services/Restaurants/Restaurants.API/Restaurants.API.csproj"
COPY . .
WORKDIR "/src/Services/Restaurants/Restaurants.API"
RUN dotnet build "Restaurants.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Restaurants.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Restaurants.API.dll"]