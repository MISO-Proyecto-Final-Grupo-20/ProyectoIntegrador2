FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StoreFlow.Usuarios.API/StoreFlow.Usuarios.API.csproj", "StoreFlow.Usuarios.API/"]
RUN dotnet restore "StoreFlow.Usuarios.API/StoreFlow.Usuarios.API.csproj"
COPY . .
WORKDIR "/src/StoreFlow.Usuarios.API"
RUN dotnet build "StoreFlow.Usuarios.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StoreFlow.Usuarios.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoreFlow.Usuarios.API.dll"]
