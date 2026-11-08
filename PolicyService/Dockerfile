FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ENV APP_HOME /opt/app
RUN mkdir -p $APP_HOME
WORKDIR $APP_HOME

COPY PricingService.API/*.csproj $APP_HOME/PricingService.API/
COPY PolicyService.API/*.csproj $APP_HOME/PolicyService.API/
COPY PolicyService/*.csproj $APP_HOME/PolicyService/
RUN cd $APP_HOME/PolicyService && dotnet restore

COPY PricingService.API $APP_HOME/PricingService.API/
COPY PolicyService.API $APP_HOME/PolicyService.API/
COPY PolicyService $APP_HOME/PolicyService/
RUN cd $APP_HOME/PolicyService && dotnet build -c Release

FROM build AS publish
WORKDIR $APP_HOME/PolicyService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5050
ENV ASPNETCORE_ENVIRONMENT=docker
COPY --from=publish /opt/app/PolicyService/out ./
ENTRYPOINT ["dotnet", "PolicyService.dll"]
