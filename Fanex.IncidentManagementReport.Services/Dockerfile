FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src

COPY ["nuget.config", "."]
COPY ["Fanex.IncidentManagementReport.Services/Fanex.IncidentManagementReport.Services.csproj", "Fanex.IncidentManagementReport.Services/"]

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore -v n -r linux-x64 "Fanex.IncidentManagementReport.Services/Fanex.IncidentManagementReport.Services.csproj"

COPY . .

WORKDIR "/src/Fanex.IncidentManagementReport.Services"

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine-25h1 AS base

WORKDIR /app

COPY --from=builder /app .
ENV TZ="Etc/GMT-8"

ENTRYPOINT ["dotnet", "Fanex.IncidentManagementReport.Services.dll"]