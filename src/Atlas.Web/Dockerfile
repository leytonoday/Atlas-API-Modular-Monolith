#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Atlas.Web/Atlas.Web.csproj", "src/Atlas.Web/"]
COPY ["src/Atlas.Shared/Atlas.Shared.csproj", "src/Atlas.Shared/"]
COPY ["src/Modules/Law/Atlas.Law.Infrastructure/Atlas.Law.Infrastructure.csproj", "src/Modules/Law/Atlas.Law.Infrastructure/"]
COPY ["src/Modules/Shared/Atlas.Shared.Infrastructure/Atlas.Shared.Infrastructure.csproj", "src/Modules/Shared/Atlas.Shared.Infrastructure/"]
COPY ["src/Modules/Shared/Atlas.Shared.Application/Atlas.Shared.Application.csproj", "src/Modules/Shared/Atlas.Shared.Application/"]
COPY ["src/Modules/Shared/Atlas.Shared.Domain/Atlas.Shared.Domain.csproj", "src/Modules/Shared/Atlas.Shared.Domain/"]
COPY ["src/Modules/Shared/Atlas.Shared.IntegratinEvents/Atlas.Shared.IntegrationEvents.csproj", "src/Modules/Shared/Atlas.Shared.IntegratinEvents/"]
COPY ["src/Modules/Shared/Atlas.Shared.Infrastructure.Razor/Atlas.Shared.Infrastructure.Razor.csproj", "src/Modules/Shared/Atlas.Shared.Infrastructure.Razor/"]
COPY ["src/Modules/Law/Atlas.Law.Application/Atlas.Law.Application.csproj", "src/Modules/Law/Atlas.Law.Application/"]
COPY ["src/Modules/Law/Atlas.Law.Domain/Atlas.Law.Domain.csproj", "src/Modules/Law/Atlas.Law.Domain/"]
COPY ["src/Modules/Plans/Atlas.Plans.Application/Atlas.Plans.Application.csproj", "src/Modules/Plans/Atlas.Plans.Application/"]
COPY ["src/Modules/Users/Atlas.Users.IntegrationEvents/Atlas.Users.IntegrationEvents.csproj", "src/Modules/Users/Atlas.Users.IntegrationEvents/"]
COPY ["src/Modules/Plans/Atlas.Plans.Domain/Atlas.Plans.Domain.csproj", "src/Modules/Plans/Atlas.Plans.Domain/"]
COPY ["src/Modules/Plans/Atlas.Plans.IntegrationEvents/Atlas.Plans.IntegrationEvents.csproj", "src/Modules/Plans/Atlas.Plans.IntegrationEvents/"]
COPY ["src/Modules/Plans/Atlas.Plans.Infrastructure/Atlas.Plans.Infrastructure.csproj", "src/Modules/Plans/Atlas.Plans.Infrastructure/"]
COPY ["src/Modules/Users/Atlas.Users.Application/Atlas.Users.Application.csproj", "src/Modules/Users/Atlas.Users.Application/"]
COPY ["src/Modules/Users/Atlas.Users.Domain/Atlas.Users.Domain.csproj", "src/Modules/Users/Atlas.Users.Domain/"]
COPY ["src/Modules/Users/Atlas.Users.Infrastructure/Atlas.Users.Infrastructure.csproj", "src/Modules/Users/Atlas.Users.Infrastructure/"]
RUN dotnet restore "./src/Atlas.Web/Atlas.Web.csproj"
COPY . .
WORKDIR "/src/src/Atlas.Web"
RUN dotnet build "./Atlas.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Atlas.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Atlas.Web.dll"]