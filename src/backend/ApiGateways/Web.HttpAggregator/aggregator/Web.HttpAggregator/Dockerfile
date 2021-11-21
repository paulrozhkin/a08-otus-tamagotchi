#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateways/Web.HttpAggregator/aggregator/Web.HttpAggregator/Web.HttpAggregator.csproj", "ApiGateways/Web.HttpAggregator/aggregator/Web.HttpAggregator/"]
COPY ["Core/Domain.Core/Domain.Core.csproj", "Core/Domain.Core/"]
COPY ["Core/Infrastructure.Core/Infrastructure.Core.csproj", "Core/Infrastructure.Core/"]
RUN dotnet restore "ApiGateways/Web.HttpAggregator/aggregator/Web.HttpAggregator/Web.HttpAggregator.csproj"
COPY . .
WORKDIR "/src/ApiGateways/Web.HttpAggregator/aggregator/Web.HttpAggregator"
RUN dotnet build "Web.HttpAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.HttpAggregator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.HttpAggregator.dll"]