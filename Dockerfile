FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["WorkoutAPI/WorkoutAPI.csproj", "WorkoutAPI/"]
COPY ["Workout.Application/Workout.Application.csproj", "Workout.Application/"]
COPY ["Workout.Infrastructure/Workout.Infrastructure.csproj", "Workout.Infrastructure/"]
COPY ["Workout.Domain/Workout.Domain.csproj", "Workout.Domain/"]

RUN dotnet restore "WorkoutAPI/WorkoutAPI.csproj"
COPY . .

WORKDIR "/src/WorkoutAPI"
RUN dotnet build "WorkoutAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "WorkoutAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health/live || exit 1

ENTRYPOINT ["dotnet", "WorkoutAPI.dll"]
