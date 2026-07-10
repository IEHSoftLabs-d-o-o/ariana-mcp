FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["NuGet.config", "./"]
COPY ["Ariana-Mcp.integrations/Ariana-Mcp.integrations.csproj", "Ariana-Mcp.integrations/"]
COPY ["Ariana-Mcp/Ariana-Mcp.csproj", "Ariana-Mcp/"]

RUN dotnet restore "Ariana-Mcp/Ariana-Mcp.csproj"

COPY . .

RUN dotnet build "Ariana-Mcp/Ariana-Mcp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ariana-Mcp/Ariana-Mcp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ariana-Mcp.dll"]
