version="99.0.0.1"
assemblyVersion="99.0.0"
docker build SwaggerApiExample -t swagger-api-example:latest --build-arg version=$version --build-arg assemblyVersion=$assemblyVersion
docker build SwaggerApiExample.Client -t nswag:build --build-arg version=$version --build-arg assemblyVersion=$assemblyVersion
# If you were to publish the Nuget package for the client, you'd run its build container with the appropriate params injected here:
# docker run nswag:build nuget/*.nupkg --source $NUGET_PUBLISH_SOURCE --api-key $NUGET_API_USER:$NUGET_API_KEY
docker run -p 5000:5000 swagger-api-example:latest