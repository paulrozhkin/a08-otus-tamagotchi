#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/OrderQueue/OrderQueue.API/OrderQueue.API.csproj", "Services/OrderQueue/OrderQueue.API/"]
COPY ["Services/OrderQueue/OrderQueue.Domain/OrderQueue.Core.csproj", "Services/OrderQueue/OrderQueue.Domain/"]
COPY ["Core/Infrastructure.Core/Infrastructure.Core.csproj", "Core/Infrastructure.Core/"]
COPY ["Core/Domain.Core/Domain.Core.csproj", "Core/Domain.Core/"]
COPY ["Services/OrderQueue/OrderQueue.DataAccess/OrderQueue.DataAccess.csproj", "Services/OrderQueue/OrderQueue.DataAccess/"]
RUN dotnet restore "Services/OrderQueue/OrderQueue.API/OrderQueue.API.csproj"
COPY . .
WORKDIR "/src/Services/OrderQueue/OrderQueue.API"
RUN dotnet build "OrderQueue.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderQueue.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderQueue.API.dll"]