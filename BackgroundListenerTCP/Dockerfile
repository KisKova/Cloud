FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BackgroundListenerTCP/BackgroundListenerTCP.csproj", "BackgroundListenerTCP/"]
RUN dotnet restore "BackgroundListenerTCP/BackgroundListenerTCP.csproj"
COPY . .
WORKDIR "/src/BackgroundListenerTCP"
RUN dotnet build "BackgroundListenerTCP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackgroundListenerTCP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackgroundListenerTCP.dll"]
