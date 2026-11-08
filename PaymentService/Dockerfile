FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ENV APP_HOME /opt/app
RUN mkdir -p $APP_HOME
WORKDIR $APP_HOME

COPY PolicyService.API/*.csproj $APP_HOME/PolicyService.API/
COPY PaymentService.API/*.csproj $APP_HOME/PaymentService.API/
COPY PaymentService/*.csproj $APP_HOME/PaymentService/
RUN cd $APP_HOME/PaymentService && dotnet restore

COPY PolicyService.API $APP_HOME/PolicyService.API/
COPY PaymentService.API $APP_HOME/PaymentService.API/
COPY PaymentService $APP_HOME/PaymentService/
RUN cd $APP_HOME/PaymentService && dotnet build -c Release

FROM build AS publish
WORKDIR $APP_HOME/PaymentService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5070
ENV ASPNETCORE_ENVIRONMENT=docker
COPY --from=publish /opt/app/PaymentService/out ./
ENTRYPOINT ["dotnet", "PaymentService.dll"]
