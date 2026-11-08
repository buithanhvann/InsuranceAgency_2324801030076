FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ENV APP_HOME /opt/app
RUN mkdir -p $APP_HOME
WORKDIR $APP_HOME

COPY PolicyService.API/*.csproj $APP_HOME/PolicyService.API/
COPY PolicySearchService.API/*.csproj $APP_HOME/PolicySearchService.API/
COPY PolicySearchService/*.csproj $APP_HOME/PolicySearchService/
RUN cd $APP_HOME/PolicySearchService && dotnet restore

COPY PolicyService.API $APP_HOME/PolicyService.API/
COPY PolicySearchService.API $APP_HOME/PolicySearchService.API/
COPY PolicySearchService $APP_HOME/PolicySearchService/
RUN cd $APP_HOME/PolicySearchService && dotnet build -c Release

FROM build AS publish
WORKDIR $APP_HOME/PolicySearchService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5065
ENV ASPNETCORE_ENVIRONMENT=docker
COPY --from=publish /opt/app/PolicySearchService/out ./
ENTRYPOINT ["dotnet", "PolicySearchService.dll"]
