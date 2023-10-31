FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SmartHomeControllers/SmartHomeControllers.csproj", "SmartHomeControllers/"]
RUN dotnet restore "SmartHomeControllers/SmartHomeControllers.csproj"
COPY . .
WORKDIR "/src/SmartHomeControllers"
RUN dotnet build "SmartHomeControllers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHomeControllers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartHomeControllers.dll"]
