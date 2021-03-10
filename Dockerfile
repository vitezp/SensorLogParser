FROM mcr.microsoft.com/dotnet/sdk:5.0 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY CmgLogParser.Console/*.csproj /build/CmgLogParser.Console/
COPY CmgLogParser.Domain/*.csproj /build/CmgLogParser.Domain/
COPY CmgLogParser/*.csproj /build/CmgLogParser/
COPY CmgLogProducer/*.csproj /build/CmgLogProducer/

RUN dotnet restore /build/CmgLogParser.Console/CmgLogParser.Console.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./CmgLogParser.Console/CmgLogParser.Console.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "CmgLogParser.Console.dll"] 