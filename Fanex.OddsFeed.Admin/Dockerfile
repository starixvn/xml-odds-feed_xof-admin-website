FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src

COPY ["Fanex.OddsFeed.Resources/Fanex.OddsFeed.Resources.csproj", "Fanex.OddsFeed.Resources/"]
COPY ["Fanex.OddsFeed.Core/Fanex.OddsFeed.Core.csproj", "Fanex.OddsFeed.Core/"]
COPY ["Fanex.OddsFeed.Models/Fanex.OddsFeed.Models.csproj", "Fanex.OddsFeed.Models/"]
COPY ["Fanex.OddsFeed.Criteria/Fanex.OddsFeed.Criteria.csproj", "Fanex.OddsFeed.Criteria/"]
COPY ["Fanex.OddsFeed.Services/Fanex.OddsFeed.Services.csproj", "Fanex.OddsFeed.Services/"]
COPY ["Fanex.OddsFeed.Controllers/Fanex.OddsFeed.Controllers.csproj", "Fanex.OddsFeed.Controllers/"]
COPY ["Fanex.OddsFeed.Admin/Fanex.OddsFeed.Admin.csproj", "Fanex.OddsFeed.Admin/"]

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore -v n "Fanex.OddsFeed.Admin/Fanex.OddsFeed.Admin.csproj"

COPY ["Fanex.OddsFeed.Resources/", "Fanex.OddsFeed.Resources/"]
COPY ["Fanex.OddsFeed.Core/", "Fanex.OddsFeed.Core/"]
COPY ["Fanex.OddsFeed.Models/", "Fanex.OddsFeed.Models/"]
COPY ["Fanex.OddsFeed.Criteria/", "Fanex.OddsFeed.Criteria/"]
COPY ["Fanex.OddsFeed.Services/", "Fanex.OddsFeed.Services/"]
COPY ["Fanex.OddsFeed.Controllers/", "Fanex.OddsFeed.Controllers/"]
COPY ["Fanex.OddsFeed.Admin/", "Fanex.OddsFeed.Admin/"]

WORKDIR /src/Fanex.OddsFeed.Admin

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-slim AS base

RUN apt-get update \
    && apt-get -y install libfontconfig1 \
    && rm -rf /var/lib/apt/lists/* /var/cache/apt/archives/*

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

WORKDIR /app
COPY --from=builder /app .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.OddsFeed.Admin.dll"]