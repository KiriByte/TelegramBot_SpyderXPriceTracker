FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TelegramBot_SpyderXPriceTracker.csproj", "./"]
RUN dotnet restore "TelegramBot_SpyderXPriceTracker.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TelegramBot_SpyderXPriceTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelegramBot_SpyderXPriceTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramBot_SpyderXPriceTracker.dll"]
