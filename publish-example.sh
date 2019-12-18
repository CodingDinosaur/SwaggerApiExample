# This file is an example of what a publish script would look like
# In theory, you want everything to build on all builds (including CI builds), so that if there's a problem, you'll know
# However, we only want to publish our assets (in this case, docker images and nuget packages) on a certified build
# Thus, you can configure your build pipeline to only run this script in the event a publish-worthy build is generated
# There are a few environment variables here that would be specific to your setup, that you would want to inject via your build system

#!/bin/bash -e
set -e

echo $PWD

docker version

# How you get the version is up to you, but you'll want it to match the version from the build script
version=$VERSION_INJECTED_SOMEHOW

# Publish client package to Nuget repository
# We first force remove any old publish container that might be running
docker rm -f nswag 2>/dev/null || true
docker run nswag:build nuget/*.nupkg --source $NUGET_PUBLISH_SOURCE --api-key $NUGET_API_USER:$NUGET_API_KEY

# Login to docker hub
docker login -u "${DOCKER_HUB_USERNAME}" -p "${DOCKER_HUB_PASSWORD}"

# Tag image for push to docker hub
docker tag swagger-api-example:build codingdinosaur/examples/swagger-api-example:$version
docker tag swagger-api-example:build codingdinosaur/examples/swagger-api-example:latest

# Push image to docker hub
docker push codingdinosaur/examples/swagger-api-example:$version

# This makes sure the package pushing container has finished
docker wait nswag

# Remove local images
# If an image has already been removed, we don't want to fail the build - this is just bonus cleanup
set +e
docker rmi -f codingdinosaur/examples/swagger-api-example:latest 2>/dev/null
docker rmi -f codingdinosaur/examples/swagger-api-example:$version 2>/dev/null
docker rmi -f swagger-api-example:build 2>/dev/null
docker rm -f nswag 2>/dev/null
docker rmi -f nswag:build 2>/dev/null