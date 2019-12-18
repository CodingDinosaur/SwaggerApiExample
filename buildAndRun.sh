version="99.0.0.1"
assemblyVersion="99.0.0"
docker build SwaggerApiExample -t swagger-api-example:build --build-arg version=$version --build-arg assemblyVersion=$assemblyVersion
# We don't *really* need to build the nuget package image, but it's here for completeness.
# If you want to see what we would do with the client image, check out publish-example.sh
docker build SwaggerApiExample.Client -t nswag:build --build-arg version=$version --build-arg assemblyVersion=$assemblyVersion
docker run -p 5000:5000 swagger-api-example:build