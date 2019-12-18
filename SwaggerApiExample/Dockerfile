FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM node:stretch-slim as clientBuild
WORKDIR /client
COPY /ClientApp .
RUN rm -rf dist
RUN npm install
RUN $(npm bin)/ng build --prod

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY . .
RUN rm -rf ClientApp
ARG version
ARG assemblyVersion
RUN dotnet publish "SwaggerApiExample.csproj" -c Release -o /app/publish -p:PackageVersion=$version -p:Version=$version -p:AssemblyVersion=$assemblyVersion

FROM base AS final
EXPOSE 5000
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=clientBuild /client/dist ClientApp/dist
ENV ASPNETCORE_URLS "http://+:5000"
RUN useradd app
USER app
ENTRYPOINT ["dotnet", "SwaggerApiExample.dll"]