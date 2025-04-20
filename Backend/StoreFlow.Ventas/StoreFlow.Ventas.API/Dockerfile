FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StoreFlow.Ventas/StoreFlow.Ventas.API/StoreFlow.Ventas.API.csproj", "StoreFlow.Ventas/StoreFlow.Ventas.API/"]
RUN dotnet restore "StoreFlow.Ventas/StoreFlow.Ventas.API/StoreFlow.Ventas.API.csproj"
COPY . .
WORKDIR "/src/StoreFlow.Ventas/StoreFlow.Ventas.API"
RUN dotnet build "./StoreFlow.Ventas.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StoreFlow.Ventas.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoreFlow.Ventas.API.dll"]
