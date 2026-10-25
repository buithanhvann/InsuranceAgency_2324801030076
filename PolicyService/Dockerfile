FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PolicyService/PolicyService.csproj", "PolicyService/"]
COPY ["PolicyService.API/PolicyService.API.csproj", "PolicyService.API/"]
COPY ["PolicyService.Implementation/PolicyService.Implementation.csproj", "PolicyService.Implementation/"]
RUN dotnet restore "PolicyService/PolicyService.csproj"
COPY . .
WORKDIR "/src/PolicyService"
RUN dotnet build "PolicyService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PolicyService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PolicyService.dll"]