#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Menu/Menu.API/Menu.API.csproj", "Services/Menu/Menu.API/"]
COPY ["Services/Menu/Menu.Domain/Menu.Domain.csproj", "Services/Menu/Menu.Domain/"]
COPY ["Core/Domain.Core/Domain.Core.csproj", "Core/Domain.Core/"]
COPY ["Core/Infrastructure.Core/Infrastructure.Core.csproj", "Core/Infrastructure.Core/"]
COPY ["Services/Menu/Menu.Infrastructure/Menu.Infrastructure.csproj", "Services/Menu/Menu.Infrastructure/"]
RUN dotnet restore "Services/Menu/Menu.API/Menu.API.csproj"
COPY . .
WORKDIR "/src/Services/Menu/Menu.API"
RUN dotnet build "Menu.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Menu.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Menu.API.dll"]