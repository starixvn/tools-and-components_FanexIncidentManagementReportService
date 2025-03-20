FROM hub.nexdev.net/base/dotnet/core/sdk:6.0-alpine AS builder

WORKDIR /src

COPY ["nuget.config", "."]
COPY ["Fanex.IncidentManagementReport.Services/Fanex.IncidentManagementReport.Services.csproj", "Fanex.IncidentManagementReport.Services/"]

RUN dotnet restore "Fanex.IncidentManagementReport.Services/Fanex.IncidentManagementReport.Services.csproj"

COPY . .

WORKDIR "/src/Fanex.IncidentManagementReport.Services"

RUN dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:6.0-alpine-25h1 AS base

WORKDIR /app

COPY --from=builder /app .
ENV TZ="Asia/Taipei"

ENTRYPOINT ["dotnet", "Fanex.IncidentManagementReport.Services.dll"]