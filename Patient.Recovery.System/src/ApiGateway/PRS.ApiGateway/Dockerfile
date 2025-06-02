FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/ApiGateway/PRS.ApiGateway/PRS.ApiGateway.csproj", "src/ApiGateway/PRS.ApiGateway/"]
RUN dotnet restore "src/ApiGateway/PRS.ApiGateway/PRS.ApiGateway.csproj"
COPY . .

WORKDIR /src/src/ApiGateway/PRS.ApiGateway

RUN dotnet build "PRS.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PRS.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRS.ApiGateway.dll"]
