FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BaseAppSql/src/Backend/WebAPI/Web.API/Web.API.csproj", "BaseAppSql/src/Backend/WebAPI/Web.API/"]
COPY ["BaseAppSql/src/Backend/Core/Application/Application.csproj", "BaseAppSql/src/Backend/Core/Application/"]
COPY ["BaseAppSql/src/Backend/Shared/Shared/Shared.csproj", "BaseAppSql/src/Backend/Shared/Shared/"]
COPY ["BaseAppSql/src/Backend/Core/Domain/Domain.csproj", "BaseAppSql/src/Backend/Core/Domain/"]
COPY ["BaseAppSql/src/Backend/Infrastructure/Infrastructure/Infrastructure.csproj", "BaseAppSql/src/Backend/Infrastructure/Infrastructure/"]
COPY ["BaseAppSql/src/Backend/Infrastructure/Persistence/Persistence.csproj", "BaseAppSql/src/Backend/Infrastructure/Persistence/"]
RUN dotnet restore "./BaseAppSql/src/Backend/WebAPI/Web.API/Web.API.csproj"
COPY . .
WORKDIR "/src/BaseAppSql/src/Backend/WebAPI/Web.API"
RUN dotnet build "./Web.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


# SSL sertifikalarini kopyala
COPY ["src/BaseAppSql/Backend/WebAPI/Web.API/ssl", "/app/ssl/"]



ENTRYPOINT ["dotnet", "Web.API.dll"]