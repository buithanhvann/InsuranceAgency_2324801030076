FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5035

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DashboardService/DashboardService.csproj", "DashboardService/"]
COPY ["DashboardService.Api/DashboardService.Api.csproj", "DashboardService.Api/"]
COPY ["DashboardService.Implementation/DashboardService.Implementation.csproj", "DashboardService.Implementation/"]
COPY ["PolicyService.API/PolicyService.API.csproj", "PolicyService.API/"]
RUN dotnet restore "DashboardService/DashboardService.csproj"
COPY . .
WORKDIR "/src/DashboardService"
RUN dotnet build "DashboardService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DashboardService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5035
ENV ASPNETCORE_ENVIRONMENT=docker
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DashboardService.dll"]
