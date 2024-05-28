FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Deliveries.Data/Deliveries.Data.csproj"
RUN dotnet build "src/Deliveries.Data/Deliveries.Data.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Deliveries.Data/Deliveries.Data.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
