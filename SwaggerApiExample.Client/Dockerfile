# Stage 1 (NSwag)
FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster as nswag
WORKDIR /app
COPY . .
COPY --from=swagger-api-example:latest /app SwaggerApiExample/app
RUN dotnet restore && \
    dotnet build -t:NSwag --configuration Release
ARG version
ARG assemblyVersion
RUN dotnet pack --configuration Release -p:PackageVersion=$version -p:Version=$version -p:AssemblyVersion=$assemblyVersion -o ./nuget/
COPY . .
ENTRYPOINT [ "dotnet", "nuget", "push" ]