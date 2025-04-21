FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StoreFlow.Inventarios.API/StoreFlow.Inventarios.API.csproj", "StoreFlow.Inventarios.API/"]
RUN dotnet restore "./StoreFlow.Inventarios.API/StoreFlow.Inventarios.API.csproj"
COPY . .
WORKDIR "/src/StoreFlow.Inventarios.API"
RUN dotnet build "./StoreFlow.Inventarios.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StoreFlow.Inventarios.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoreFlow.Inventarios.API.dll"]
