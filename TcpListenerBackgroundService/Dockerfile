FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./TcpListenerBackgroundService.csproj", "TcpListenerBackgroundService/"]
COPY ["./Program.cs", "TcpListenerBackgroundService/"]
COPY ["./TcpBackgroundListener.cs", "TcpListenerBackgroundService/"]
COPY ["./deployment.yaml", "TcpListenerBackgroundService/"]
COPY ["./service.yaml", "TcpListenerBackgroundService/"]

RUN dotnet restore "TcpListenerBackgroundService/TcpListenerBackgroundService.csproj"
COPY . .
WORKDIR "/src/TcpListenerBackgroundService"
RUN dotnet build "TcpListenerBackgroundService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TcpListenerBackgroundService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TcpListenerBackgroundService.dll"]