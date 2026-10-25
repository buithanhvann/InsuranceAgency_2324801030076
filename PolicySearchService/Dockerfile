FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PolicySearchService/PolicySearchService.csproj", "PolicySearchService/"]
COPY ["PolicySearchService.API/PolicySearchService.API.csproj", "PolicySearchService.API/"]
COPY ["PolicySearchService.Implementation/PolicySearchService.Implementation.csproj", "PolicySearchService.Implementation/"]
RUN dotnet restore "PolicySearchService/PolicySearchService.csproj"
COPY . .
WORKDIR "/src/PolicySearchService"
RUN dotnet build "PolicySearchService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PolicySearchService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PolicySearchService.dll"]