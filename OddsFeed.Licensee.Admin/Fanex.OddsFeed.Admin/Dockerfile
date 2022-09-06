# escape=`
FROM hub.nexdev.net/base/rocketor/buildservice AS builder

ARG DEPLOY_ENV

SHELL ["cmd", "/S", "/C"]

WORKDIR /build

COPY *.sln .

COPY OddsFeed.Licensee.Admin\Fanex.OddsFeed.Core\*.csproj OddsFeed.Licensee.Admin\Fanex.OddsFeed.Core\
COPY OddsFeed.Licensee.Admin\Fanex.OddsFeed.Core\packages.config OddsFeed.Licensee.Admin\Fanex.OddsFeed.Core\

COPY OddsFeed.Licensee.Admin\Fanex.OddsFeed.Framework\*.csproj OddsFeed.Licensee.Admin\Fanex.OddsFeed.Framework\
COPY OddsFeed.Licensee.Admin\Fanex.OddsFeed.Framework\packages.config OddsFeed.Licensee.Admin\Fanex.OddsFeed.Framework\

COPY OddsFeed.Licensee.Admin\Fanex.OddsFeed.Admin\*.csproj OddsFeed.Licensee.Admin\Fanex.OddsFeed.Admin\
COPY OddsFeed.Licensee.Admin\Fanex.OddsFeed.Admin\packages.config OddsFeed.Licensee.Admin\Fanex.OddsFeed.Admin\

RUN nuget.exe restore XMLOddsFeed.sln

COPY OddsFeed.Licensee.Admin\Fanex.OddsFeed.Core\ OddsFeed.Licensee.Admin\Fanex.OddsFeed.Core\
COPY OddsFeed.Licensee.Admin\Fanex.OddsFeed.Framework\ OddsFeed.Licensee.Admin\Fanex.OddsFeed.Framework\
COPY OddsFeed.Licensee.Admin\Fanex.OddsFeed.Admin\ OddsFeed.Licensee.Admin\Fanex.OddsFeed.Admin\

RUN MSBuild.exe "OddsFeed.Licensee.Admin\Fanex.OddsFeed.Admin\Fanex.OddsFeed.Admin.csproj" `
    /t:Build;WebPublish /m `
    /p:DebugType=None `
    /p:Configuration=Release `
    /p:WebPublishMethod=FileSystem `
    /p:TransformWebConfigEnabled=False `
    /p:LocalizedBuild=true `
    /p:AutoParameterizationWebConfigConnectionStrings=False `
    /p:MarkWebConfigAssistFilesAsExclude=false `
    /p:publishUrl="C:\Compiled" `
    /consoleloggerparameters:ErrorsOnly

WORKDIR C:\Compiled

# Transform config
RUN echo "%DEPLOY_ENV%" `
    && xcopy "Web.%DEPLOY_ENV%.config" "Web.config" /Y`
    && xcopy "App_Data\SpNames.%DEPLOY_ENV%.xml" "App_Data\SpNames.xml" /Y

FROM hub.nexdev.net/base/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019 AS base

WORKDIR C:\inetpub\wwwroot

COPY --from=builder C:\Compiled .

RUN dir .\bin\

ENTRYPOINT ["C:\\LogMonitor\\LogMonitor.exe", "C:\\ServiceMonitor.exe", "w3svc"]
