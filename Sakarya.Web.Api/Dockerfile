FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /src

COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Entities/Entities.csproj", "Entities/"]
RUN dotnet restore "WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/build

FROM runtime AS final
WORKDIR /app
COPY --from=publish /app/build .
#ENV ASPNETCORE_URLS="http://*:44375"
ENV ASPNETCORE_ENVIRONMENT Staging
ENV ASPNETCORE_ENVIRONMENT Staging
ENV MongoDbSettings:User root
ENV MongoDbSettings:User root
ENV MongoDbSettings:Password test
ENV MongoDbSettings:Host localhost
ENV MongoDbSettings:DatabaseName sakarya
ENV SeriLogConfigurations:LogstashConfiguration:Host localhost
ENV SeriLogConfigurations:LogstashConfiguration:Port 5000


ENTRYPOINT ["dotnet", "WebAPI.dll"]