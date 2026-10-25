FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PricingService/PricingService.csproj", "PricingService/"]
COPY ["PricingService.Api/PricingService.Api.csproj", "PricingService.Api/"]
COPY ["PricingService.Implementation/PricingService.Implementation.csproj", "PricingService.Implementation/"]
RUN dotnet restore "PricingService/PricingService.csproj"
COPY . .
WORKDIR "/src/PricingService"
RUN dotnet build "PricingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PricingService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PricingService.dll"]