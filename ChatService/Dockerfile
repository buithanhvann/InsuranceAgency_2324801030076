FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 4099

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ChatService/ChatService.csproj", "ChatService/"]
COPY ["ChatService.API/ChatService.API.csproj", "ChatService.API/"]
COPY ["ChatService.Implementation/ChatService.Implementation.csproj", "ChatService.Implementation/"]
COPY ["PolicyService.API/PolicyService.API.csproj", "PolicyService.API/"]
RUN dotnet restore "ChatService/ChatService.csproj"
COPY . .
WORKDIR "/src/ChatService"
RUN dotnet build "ChatService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:4099
ENV ASPNETCORE_ENVIRONMENT=docker
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatService.dll"]
