FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ENV APP_HOME /opt/app
RUN mkdir -p $APP_HOME
WORKDIR $APP_HOME

COPY ProductService.API/*.csproj $APP_HOME/ProductService.API/
COPY ProductService/*.csproj $APP_HOME/ProductService/
RUN cd $APP_HOME/ProductService && dotnet restore

COPY ProductService.API $APP_HOME/ProductService.API/
COPY ProductService $APP_HOME/ProductService/
RUN cd $APP_HOME/ProductService && dotnet build -c Release

FROM build AS publish
WORKDIR $APP_HOME/ProductService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5030
ENV ASPNETCORE_ENVIRONMENT=docker
COPY --from=publish /opt/app/ProductService/out ./
ENTRYPOINT ["dotnet", "ProductService.dll"]
