#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Menu.API/Menu.API.csproj", "Menu.API/"]
RUN dotnet restore "Menu.API/Menu.API.csproj"
COPY . .
WORKDIR "/src/Menu.API"
RUN dotnet build "Menu.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Menu.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Menu.API.dll"]