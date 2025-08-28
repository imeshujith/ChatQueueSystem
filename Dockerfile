# .NET 9 multi-stage build
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8008

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ChatQueueSystem.API/ChatQueueSystem.API.csproj", "ChatQueueSystem.API/"]
COPY ["ChatQueueSystem.Application/ChatQueueSystem.Application.csproj", "ChatQueueSystem.Application/"]
COPY ["ChatQueueSystem.Domain/ChatQueueSystem.Domain.csproj", "ChatQueueSystem.Domain/"]
COPY ["ChatQueueSystem.Infrastructure/ChatQueueSystem.Infrastructure.csproj", "ChatQueueSystem.Infrastructure/"]
RUN dotnet restore "ChatQueueSystem.API/ChatQueueSystem.API.csproj"
COPY . .
WORKDIR "/src/ChatQueueSystem.API"
RUN dotnet publish "ChatQueueSystem.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ChatQueueSystem.API.dll"]
