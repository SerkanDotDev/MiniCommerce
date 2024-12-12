FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MiniCommerce.Api/MiniCommerce.Api.csproj", "MiniCommerce.Api/MiniCommerce.Api/"]
COPY ["MiniCommerce.Domain/MiniCommerce.Domain.csproj", "MiniCommerce.Domain/"]
COPY ["MiniCommerce.Domain.Core/MiniCommerce.Domain.Core.csproj", "MiniCommerce.Domain.Core/"]
COPY ["MiniCommerce.Infra/MiniCommerce.Infra.csproj", "MiniCommerce.Infra/"]
RUN dotnet restore "./MiniCommerce.Api/MiniCommerce.Api.csproj"
COPY . .
WORKDIR "/src/MiniCommerce.Api/MiniCommerce.Api"
RUN dotnet build "./MiniCommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MiniCommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/wwwroot/profile-photos && \
    chmod 777 /app/wwwroot/profile-photos
ENTRYPOINT ["dotnet", "MiniCommerce.Api.dll"]