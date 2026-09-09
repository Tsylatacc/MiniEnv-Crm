FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /src

COPY ["MiniEnv.Api/MiniEnv.Api.csproj", "MiniEnv.Api/"]
COPY ["MiniEnv.Application/MiniEnv.Application.csproj", "MiniEnv.Application/"]
COPY ["MiniEnv.Domain/MiniEnv.Domain.csproj", "MiniEnv.Domain/"]
COPY ["MiniEnv.Infrastructure/MiniEnv.Infrastructure.csproj", "MiniEnv.Infrastructure/"]

RUN dotnet restore "MiniEnv.Api/MiniEnv.Api.csproj"

COPY . .

WORKDIR /src/MiniEnv.Api

RUN dotnet publish \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "MiniEnv.Api.dll"]