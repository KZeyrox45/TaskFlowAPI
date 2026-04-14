# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files and restore dependencies
COPY ["TaskFlowAPI.API/TaskFlowAPI.API.csproj", "TaskFlowAPI.API/"]
COPY ["TaskFlowAPI.Application/TaskFlowAPI.Application.csproj", "TaskFlowAPI.Application/"]
COPY ["TaskFlowAPI.Infrastructure/TaskFlowAPI.Infrastructure.csproj", "TaskFlowAPI.Infrastructure/"]
COPY ["TaskFlowAPI.Core/TaskFlowAPI.Core.csproj", "TaskFlowAPI.Core/"]
RUN dotnet restore "TaskFlowAPI.API/TaskFlowAPI.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/TaskFlowAPI.API"
RUN dotnet build "TaskFlowAPI.API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "TaskFlowAPI.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "TaskFlowAPI.API.dll"]
