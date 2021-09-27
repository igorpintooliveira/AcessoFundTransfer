#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TestBackend.FundTransfer.API.Consumer/TestBackend.FundTransfer.API.Consumer.csproj", "TestBackend.FundTransfer.API.Consumer/"]
COPY ["TestBackend.FundTransfer.Application/TestBackend.FundTransfer.Application.csproj", "TestBackend.FundTransfer.Application/"]
COPY ["TestBackend.FundTransfer.Domain/TestBackend.FundTransfer.Domain.csproj", "TestBackend.FundTransfer.Domain/"]
COPY ["TestBackend.FundTransfer.EventBus/TestBackend.FundTransfer.EventBus.csproj", "TestBackend.FundTransfer.EventBus/"]
COPY ["TestBackend.FundTransfer.Infrastructure.Mongo/TestBackend.FundTransfer.Infrastructure.Mongo.csproj", "TestBackend.FundTransfer.Infrastructure.Mongo/"]
COPY ["TestBackend.FundTransfer.Infrastructure/TestBackend.FundTransfer.Infrastructure.Redis.csproj", "TestBackend.FundTransfer.Infrastructure/"]
RUN dotnet restore "TestBackend.FundTransfer.API.Consumer/TestBackend.FundTransfer.API.Consumer.csproj"
COPY . .
WORKDIR "/src/TestBackend.FundTransfer.API.Consumer"
RUN dotnet build "TestBackend.FundTransfer.API.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestBackend.FundTransfer.API.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestBackend.FundTransfer.API.Consumer.dll"]
