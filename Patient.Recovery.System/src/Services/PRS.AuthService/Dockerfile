FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Services/PRS.AuthService/PRS.AuthService.csproj", "src/Services/PRS.AuthService/"]
RUN dotnet restore "src/Services/PRS.AuthService/PRS.AuthService.csproj"
COPY . .

WORKDIR /src/src/Services/PRS.AuthService

RUN dotnet build "PRS.AuthService.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PRS.AuthService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRS.AuthService.dll"]