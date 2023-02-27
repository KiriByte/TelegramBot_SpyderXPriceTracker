FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

 # Copy everything
COPY . .
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o /publish
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /publish
COPY --from=build-env /publish .

# Install Google Chrome
RUN apt-get update
RUN apt-get install wget -y
RUN wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
RUN apt-get install ./google-chrome*.deb --yes

ENV TELEGRAM_TOKEN=""

ENTRYPOINT ["dotnet", "TelegramBot_SpyderXPriceTracker.dll"]