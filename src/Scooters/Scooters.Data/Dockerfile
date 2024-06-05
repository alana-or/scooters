FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Scooters/Scooters.Data/Scooters.Data.csproj"
RUN dotnet build "src/Scooters/Scooters.Data/Scooters.Data.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Scooters/Scooters.Data/Scooters.Data.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
