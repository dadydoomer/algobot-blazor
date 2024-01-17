FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore Algobot/Server/Algobot.Server.csproj
RUN dotnet publish Algobot/Server/Algobot.Server.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "Algobot.Server.dll"]

# docker build -t algobot-blazor -f Dockerfile .
# docker-compose up