# OpenAPI 3 / Swagger API Example <!-- omit in toc -->

This repository contains an application that shows an example of creating a self-documenting HTTP-based API designed to be consumed by others unfamiliar with it.  Inside, several different approaches are taken in various methods, varying from simple methods with primitive types, to complex wire-types and mutable responses that can vary depending on the response code, and more.

This application can build, run, and serve its API for educational purposes, but it does not provide any useful functionality.  Everything just below the service layers are just silly things designed to be filler points for the API.

- [Basic Info](#basic-info)
- [Building &amp; Running](#building-amp-running)
    - [With Docker](#with-docker)
    - [With Visual Studio &amp; Node](#with-visual-studio-amp-node)
- [A Self-Documenting API](#a-self-documenting-api)
  - [Enabling Swagger &amp; Swagger UI](#enabling-swagger-amp-swagger-ui)
    - [Register Swagger Services](#register-swagger-services)
    - [Inject Swagger &amp; Swagger UI into Request Pipeline](#inject-swagger-amp-swagger-ui-into-request-pipeline)
  - [Ensuring Accurate Documentation](#ensuring-accurate-documentation)
  - [Accurately Documenting Response Types](#accurately-documenting-response-types)
    - [Direct Result Types](#direct-result-types)
    - [IActionResult / ActionResult](#iactionresult--actionresult)
    - [ActionResult&lt;T&gt; - A Hybrid](#actionresultlttgt---a-hybrid)
    - [Async Methods](#async-methods)
    - [Advanced Techniques](#advanced-techniques)
  - [Descriptions &amp; Remarks](#descriptions-amp-remarks)
    - [Generating an XML Document File](#generating-an-xml-document-file)
    - [Hooking up XML Docs to Swagger](#hooking-up-xml-docs-to-swagger)
  - [Further Reading](#further-reading)
- [NSWag Client Generation](#nswag-client-generation)
  - [NSwag Installation](#nswag-installation)
  - [NSwag Configuration](#nswag-configuration)
  - [Running NSwag using the Configuration](#running-nswag-using-the-configuration)
  - [NSwag MSBuild Target](#nswag-msbuild-target)
  - [Package Configuration](#package-configuration)
  - [Versioning](#versioning)
    - [Semantic Versioning Strategy](#semantic-versioning-strategy)
- [Docker files &amp; Build Process](#docker-files-amp-build-process)
  - [Containerized Hosting](#containerized-hosting)
  - [Containerized Building](#containerized-building)
    - [Multi-Stage Building](#multi-stage-building)
  - [Containerized Package Publishing](#containerized-package-publishing)
  - [Further Reading](#further-reading-1)

# Basic Info
- Written in C# 8.0
- Hosting application is ASP.NET in .NET Core 3
- Contains a mix of ASP.NET Web API controllers, basic static files, and an Angular SPA
- Takes advantage of the new SPA tools in ASP.NET, including integration between the ASP.NET dev server and the Angular dev server
- Uses Swashbuckle to generate Swagger documentation and Swagger UI pages

# Building & Running

### With Docker
Simply run *buildAndRun.sh*, which will build and run the docker container for you:
```bash
docker build SwaggerApiExample -t swagger-api-example:latest
docker run -p 5000:5000 swagger-api-example:latest
```

### With Visual Studio & Node
- Open the solution folder on the command line
- Restore Nuget packages via *dotnet restore*
- Go to *cd SwaggerApiExample/ClientApp*
- Restore NPM packages via *npm install*
- Open SwaggerApiExample.sln in Visual Studio
- Select the "SwaggerApiExample" build profile
- Build and run!

# A Self-Documenting API

## Enabling Swagger & Swagger UI
*Swashbuckle* is the package which drives the generation of OpenAPI 3 spec files (a.k.a. "Swagger" files and the Swagger UI.  *Swashbuckle* is a well-established package available on Nuget.Org

After installing the package, we need to register the Swagger generator into the DI container, then add both the Swagger generator and the Swagger UI to the request pipeline.

### Register Swagger Services
First, add the Swagger generation services to the DI container in your *ConfigureServices* method and set the options.  In this case, we're setting a title, an API version, and injecting XML comments (more on that later).

```csharp
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SwaggerApiExample API", Version = "v1" });
    c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "SwaggerApiExample.xml", true);
});
```

### Inject Swagger & Swagger UI into Request Pipeline
To make your application respond to requests for the swagger.json file and to load the Swagger UI, we need to inject the appropriate middleware into the HTTP request pipeline.  Like other middlware registration, this will be done in on the *IApplicationBuilder* in your *Configure* method.

Document generation is added via the *UseSwagger* extension method, and by default, this will respond to the */swagger/{version}/swagger.json* path.  

The Swagger UI is injected via the *UseSwaggerUI* extension method, and it's here that we can insert customizations to the UI layer itself.  In the below example, we'll register both the document generation middleware, and the UI middeleware, tell the UI middleware where to find the Swagger doc when it loads, and inject a custom style sheet.

```csharp
app.UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Api Example V1");
        c.InjectStylesheet("/static/css/swaggerui-dark.css");
    });
```

## Ensuring Accurate Documentation
It is critical that the Swagger docs you generate be accurate, or they are of no use to anyone who tries to interop with your API.

Tools like *Swashbuckle*, *AutoRest*, and *NSWag* use a variety of methods to determine the shape and structure of your API that generally all boil down to the code your wrote clearly describing the intent.  This is no different than how ASP.NET itself decides how to handle routing, model binding, and serialization.  For many things, there are determinations made by convention, such as:
- An action with a name that starts in "Get" and does not require any complex request payload will be interpreted as an HTTP GET method
- An action with no complex request payload, and a verb with no body, the model binder will seek route params that match parameters by name before falling back on the query string.

These convention-based approaches however, often lead to confusion for developers, and can lead to inconsistent documentation for complex cases.  This is why I generally recommend being declarative with anything that isn't immediately obvious to a reader, to ASP.NET, and to 3rd-parties -- such as marking actions with the *HttpGet* attribute, using explicit *FromRoute* and *FromQuery* attributes on parameters, as well as adding route constraints and model validation where appropriate.

Your goal should be to make the API as close to truly self-describing as possible -- **you shouldn't have to write a single line of the Swagger doc manually**.  This is why it's important to be as declarative as is practical when building your API -- you will ensure more consistent functionality, easier testing, and stronger documentation.

## Accurately Documenting Response Types

One of the easily missed parts of API documentation is the response types, because it's the one part that generally will not impact your application's runtime directly.  Once your API has properly received a request, deserialized it, ingested it, processed it, crafted a response, and serialized that -- all it does is fire the response payload back into the void.

There are now three common ways to define your responses in ASP.NET API controllers, with two of which being fully self-documenting, and one requiring a little nudge in the form of an attribtue.

### Direct Result Types
In this scenario, you define your API actions to directly return their result types.  For example:

```csharp
/// <summary>
/// Get a list of non-sensical words that sound remotely sciencey
/// </summary>
/// <param name="limit">Maximum number of words to get</param>
/// <returns>Nothing useful, honestly</returns>
[HttpGet("words")]
public List<StringSegment> GetFancySoundingWords([FromQuery] int limit)
{
    var words = _scienceManager.GetScienceyWords(limit);
    return words.ToList();
}
```

**This is self-documenting** -- meaning Swashbuckle and other tools will know what to put in your Swagger doc for your response type.  Unfortunately, this approach doesn't give you much control over the response other than to provide the payload.

### IActionResult / ActionResult
Because direct result types mean you can't control the response other than the payload -- for example, you can't conditionally decide to provide a 400 or a 504, or add response headers -- a common approach is to instead define your actions as returning **IActionResult** (or its concrete sibling, *ActionResult*)

**But now Swagger tools don't know what your return type is**, so it doesn't list one in your Swagger file.  This can be easily resolved using the **Produces** or **ProducesResponseType** attributes:

```csharp
/// <summary>
/// Get a list of non-sensical words that sound remotely sciencey
/// </summary>
/// <param name="limit">Maximum number of words to get</param>
/// <returns>Nothing useful, honestly</returns>
[HttpGet("words")]
[Produces(typeof(List<StringSegment>))]
public IActionResult GetFancySoundingWords([FromQuery] int limit)
{
    var words = _scienceManager.GetScienceyWords(limit);
    return Ok(words.ToList());
}
```
For an action that always returns the same type, use the *Produces* attribute.  If the action can conditionally return different types based on the result code, use *ProducesResponseType*:

```csharp
/// <summary>
/// Get a list of non-sensical words that sound remotely sciencey
/// </summary>
/// <param name="limit">Maximum number of words to get</param>
/// <returns>Nothing useful, honestly</returns>
[HttpGet("words")]
[ProducesResponseType(typeof(List<StringSegment>), 200)]
[ProducesResponseType(typeof(BadArgumentInfo), 400)]
public IActionResult GetFancySoundingWords([FromQuery] int limit)
{
    if (limit > 500)
    {
        return BadRequest(new BadArgumentInfo
        {
            Argument = nameof(limit),
            Message = "Limit cannot exceed 500"
        });
    }
    
    var words = _scienceManager.GetScienceyWords(limit);
    return Ok(words.ToList());
}
```

>*Author's Note:*
>
>**For compatibility reasons, and so that your consumers don't grow too bitter, I personally reccomend against having multiple response types come back from the same action!**

#### A Note on Async Methods
If the method is async and returns a *Task\<T\>*, then your attribute should only contain type *T* and **not** the *Task* type.  The Task is internal to your application and not part of the API contract your are establishing.

### ActionResult\<T\> - A Hybrid
In ASP.NET Core 2.1, Microsoft added a generic version of *ActionResult*, giving us *ActionResult\<T\>*.  Using this, we can get both the self-documenting behavior of direct types, but with the added access to the response parameters.  This is a good compromise as long as you don't need different response types conditionally, as in the *ProducesResponseType* example above.

```csharp
/// <summary>
/// Get a list of non-sensical words that sound remotely sciencey
/// </summary>
/// <param name="limit">Maximum number of words to get</param>
/// <returns>Nothing useful, honestly</returns>
[HttpGet("words")]
public ActionResult<List<StringSegment>> GetFancySoundingWords([FromQuery] int limit)
{
    var words = _scienceManager.GetScienceyWords(limit);
    return Ok(words.ToList());
}
```

### Async Methods
A properly implemented async / await controller method will not change the documentation behavior listed above.  Whether a method is async on the .NET side or not **does not change the API contract** -- it's an implementation detail, and as such is intentionally not represented in the Swagger documentation (other than in names &  and comments left by developers).

To apply async / await to the above examples, the return types would simply be wrapped in a *Task\<T\>*, and this would apply equally to all three (though when using *Produces* attributes, you'd simply specify type *T*).

### Advanced Techniques
These aren't the only ways to signal your intent to your documentation.  Using the API Explorer's *Conventions* system, you can even define your own implicit rules for document generation if needed.

For more info on this topic, see:
https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-3.1

## Descriptions & Remarks
Just as important as naming and API shape are descriptions of actions, explanations of types, and remarks about how to use each action.

Swagger and Swagger UI provide a number of mechanism to display this kind of contextual information, and tools like Swashbuckle can import these from your code's XML doc comments.

It is strongly recommended that all of your API methods, public-facing types, and properties have at least an XML doc comment with a *summary* section.  It is also recommended to use *param* blocks for methods, and *remarks* for more in-depth comments.  You can see an example of this on the *MeeseeksController.StartTasks* action.

To ensure that your Swagger documents & UI benefit from these comments, you will need to make sure that your comments are exported to XML as part of the build, and that these are then imported when bootstrapping the Swagger generator in *Startup*.

### Generating an XML Document File
In Visual Studio, you can right-click on a project and select "properties", then find the setting for generating an XML document file in the "Build" section.  If you go this route, make sure to apply this to your release configuration or all configurations, and not just your debug configuration.

However, you can also add a one-liner to your *csproj* file that will generate this file for all of your build configurations, and output it directly to the build's output folder, rather than dumping it in the solution or having to rely on an absolute path.

Within the top-level *PropertyGroup* (with no conditional expressions on it), add an element similar to the following:
```xml
<DocumentationFile>bin\$(Configuration)\$(TargetFramework)\SwaggerApiExample.xml</DocumentationFile>
```

### Hooking up XML Docs to Swagger
In your *Startup.ConfigureServices* method, when registering Swashbuckle's services you will already have a lambda for modifying the Swagger generator options, currently setting the title and API version.  On that same options object, you'll want to invoke *IncludeXmlComments*, along with the path that the XML file can be found.  For example:

```csharp
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SwaggerApiExample API", Version = "v1" });
    c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "SwaggerApiExample.xml", true);
});
```
Note that this is the **file system path**, not an HTTP path, and a common error is to address paths that are only valid in one environment -- such as by hard-coding an absolute path, or by using path addressing only valid in the developer's local file system (e.g., using back slashes in Windows then trying to run on Linux).

In the above case, by obtaining the base directory at runtime, then whether running on Windows, Linux, or within a Docker container, the XML file will be correctly located.

## Further Reading

Check out:

- Microsoft's article on specifying action return types:  https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1
- Custom Conventions:  https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-3.1
- Swashbuckle Github:  https://github.com/domaindrivendev/Swashbuckle
- NSWag Github:  https://github.com/RicoSuter/NSwag

# NSWag Client Generation

An SDK with stronly typed C# contracts can be automatically generated at build-time using NSwag.  NSwag will first use the compiled host assembly to determine a the Open API description, then use a CSharp code generator to construct generated classes, operations, and clients.

For full information about NSwag, see: http://nswag.org

## NSwag Installation
NSwag client generation will NOT occur automatically during local builds.  If you would like to generate the client assembly manually, you will need to install NSwag.

There are several options for installation (NPM, MSI, Chocolatey, Zip archive).  See installation instructions here:  https://github.com/RSuter/NSwag/wiki/CommandLine

## NSwag Configuration
NSwag has a lot of options and currently they are all set up using the nswag.json file in the root of the repository.  The simplest way to modify this is using NSwagStudio, the windows desktop application for editing and configurating NSwag.  NSwagStudio will be installed automatically if you install the windows version of NSwag as descrbied above.  

You can also find a plethora of documentation on the NSwag Wiki:  
  - https://github.com/RSuter/NSwag/wiki
  - https://github.com/RSuter/NSwag/wiki/NSwag-Configuration-Document

You'll always want to make sure that the namespaces and package details are to your liking, since the client will generate a package for use by your consumers.

## Running NSwag using the Configuration
You can utilize the nswag.json file to generate the client via:
```bash
nswag run
```

The nswag run command will automatically locate the config file if it is named nswag.json.

## NSwag MSBuild Target
In this example, the NSWag.MSBuild package is used to enable build-time client generation.  Because we only want this to happen when our build scripts are running and not during local development builds, we create a custom target in the csproj file like so:

```xml
<Target Name="NSwag">
    <Message Text="Running NSWag Client Code Generation..." Importance="High" />
    <Exec Command="$(NSwagExe_Core30) run ./nswag.json" />
</Target>
```

Then, when we want to execute this at build-time, we simply run the NSwag target:
```bash
dotnet restore && \
    dotnet build -t:NSwag --configuration Release
```

In the configuration we've specified in our *nswag.json*, NSwag will output the generated code as *NSWagGeneratedCode.cs*.  Because the new csproj format is compiler-inclusive by default (meaning, it'll compile all .cs files it finds), you can follow the NSwag generation step with a regular build or publish to generate a compiled assembly containing the generated client.

## Package Configuration
Because our client csproj file will be used to generate the assembly *and* to pack the package, that's where we can store the basic meta-data about the package we're going to generate.  You can edit the project properties in Visual Studio to manage these, or edit the csproj directly.

```xml
<PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <PackageId>SwaggerApiExample.Client</PackageId>
    <Version>1.0.0</Version>
    <AssemblyVersion>1.0.0.0</AssemblyVersion>
    <FileVersion>1.0.0.0</FileVersion>
    <Authors>Gilbert Samuelian</Authors>
    <Company>Blizzard Entertainment</Company>
    <Description>Auto-generated client for SwaggerApiExample, driving Meeseeks tasks and basic Dinosaur-related information.</Description>
    <Copyright>2019</Copyright>
    <RepositoryType>Git</RepositoryType>
    <PackageIconUrl>https://avatars0.githubusercontent.com/u/26150969?s=400&u=abfbf0a8b378bd3571ec17f0cfcd22be6de3cb44&v=4</PackageIconUrl>
    <RepositoryUrl>https://github.com/CodingDinosaur/SwaggerApiExample</RepositoryUrl>
</PropertyGroup>
```

Note that you can also have a .nuspec file, or override most of these on the command-line at build time.

## Versioning

Proper versioning of your assemblies and packages is critical for your consumers.  In this example, the versions are variables which are then passed into the build process from the command-line scripts.

```bash
docker build SwaggerApiExample -t swagger-api-example:build --build-arg version=$version --build-arg assemblyVersion=$assemblyVersion
```
```dockerfile
ARG version
ARG assemblyVersion
RUN dotnet publish "SwaggerApiExample.csproj" -c Release -o /app/publish -p:PackageVersion=$version -p:Version=$version -p:AssemblyVersion=$assemblyVersion
```

The reason for this is that the build number is generally something that will be determined by your pipeline or CI process, not something that you would hard-code into your scripts or input at design-time.  There are three version numbers we want to make sure to get right:
- Assembly Version:  Generally only 3 positions (A.B.C), with 16-bit numeric values only.  Used primarily at run-time for assembly binding.
- Product / File Version:  Up to 4 positions (A.B.C.D), treated more like a string.  Good for expressing individual build versions between assembly version changes.
- Package version:  The Nuget package version can be 4 positions with alphanumerics and suffixes.  You should match the product version first, add suffixes second.
  - Packages with suffixes are considered to be *pre-release* packages

In this example repo, the versions are hard-coded in the *buildAndRun* script, but in a real-world scenario, you'd want to pass these in from your build pipeline.  (For example, we often compute them based on the last Git release + number of commits since that release)

For example:
- Assembly Version:  2.0.1  (Last Github release was 2.0.1)
- Product Version:  2.0.1.15  (There have been 15 additional commits since the 2.0.1 release)
- Package Version:  2.0.1.15-prerelease  (The package will only show up if someone looks for pre-release packages)

### Semantic Versioning Strategy

Before you go too deep down the rabbit hole of building your system, you'll want to decide on a good versioning strategy that you and your team can stick to.  

If your application contains a forward-facing API layer of any kind, then it is **strongly** recommended you consider adhering to [Semantic Versioning standards (https://semver.org)](https://semver.org/).

# Docker files & Build Process

You may be wondering, "what's with all the Docker files", or "why do you build it like that" -- so I want to give a quick overview of the build strategy in place here.

## Containerized Hosting

The test application is hosted in a Docker container.  If you aren't familiar with Docker containers, or you've only heard about them, then I strongly suggest you check out some online resources or a Pluralsight course -- Docker can completely revolutionize the way you develop and host applications.

By hosting the application in a Docker container, we can host it essentially anywhere and make changes to its hosting without having to worry about the application itself.  If we want to start out on bare metal, move to a dedicated VM, then later move to a fully orchestrated Kubernetes cluster, we won't have to change *anything* about the application itself to make that work.  Furthermore, we know it'll always work the same way -- there won't be any qwirks specific to the hosting OS itself (like differences in encodings, line endings, localization, packages, etc).  Everything the application needs to run anwywhere is fully self-contained in the light weight Docker image.

## Containerized Building

This one you may not have seen as often, but the same logic behind containzerized hosting has lef my team to containerized building.  We no longer have to worry about changes to build servers, switching CI platforms, or multitudes of other problems we used to have routinely when at the mercy of unforgiving build systems like Jenkins.

Node is a notoriously painful dependency on build servers, but we never have to worry about incorrect or colliding node versions when we build in a container:
```dockerfile
FROM node:stretch-slim as clientBuild
WORKDIR /client
COPY /ClientApp .
RUN rm -rf dist
RUN npm install
RUN $(npm bin)/ng build --prod
```

It also means that the ideal environment configuration to build, any packages, the correct versoins of MSBuild, etc -- it's all handled for us by Microsoft when they make the base images.  Ready to upgrade to the next version of the framework?  No worries, just change your compilation target and your base image tag, and that's it as far as compilation goes -- you will never have to remote into a builder to install SDKs again.

```dockerfile
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
```

### Multi-Stage Building

Youll notice liberal use of the "as" keyword in dockerfile FROM statements.  This is to create a multi-stage docker build.  For more information, check out:  
[Docker Documentation - Using Multi-Stage Builds](https://docs.docker.com/develop/develop-images/multistage-build/)

## Containerized Package Publishing

This is a trick that I came up with about a year and a half ago when faced with the challenge of wanting to pack and publish packages that were based on builds happening inside build containers.  Most of the solutions I saw involved doing a separate "dotnet publish" or build outside the container, but that effectively means you at least double your build time (since now you're building both inside, and out of the build container).  Additionally, it negates most of the beneficial reasons for containerized building to begin with.

The other challenge is that we don't want to publish packages on *all* builds -- for example, PR CI builds or feature branch builds shouldn't push packages to the main package repo in most cases.  Ideally however, we'd still like to *build* the packages in those cases, so that if there's a problem, it shows up.

So the idea I had, which is illustrated by the Client project in this repo, was to also build the nuget packages in a container, but then set the container's entry point to *dotnet nuget push*.  That way, you can *docker build* to generate everything and make sure the build passes, and if it's a build which should publish assets, you can *docker run* with the appropriate parameters to publish the nuget package.

```dockerfile
RUN dotnet pack --configuration Release -p:PackageVersion=$version -p:Version=$version -p:AssemblyVersion=$assemblyVersion -o ./nuget/
COPY . .
ENTRYPOINT [ "dotnet", "nuget", "push" ]
```

```bash
# Publish client package to Nuget repository
# We first force remove any old publish container that might be running
docker rm -f nswag 2>/dev/null || true
docker run nswag:build nuget/*.nupkg --source $NUGET_PUBLISH_SOURCE --api-key $NUGET_API_USER:$NUGET_API_KEY
```

You can see what this would look like in the *publishExample.sh* file.

## Further Reading
Check out:
- Dockerfile Reference:  https://docs.docker.com/engine/reference/builder/
- Docker Multi-stage builds:  https://docs.docker.com/develop/develop-images/multistage-build/
- Semantic Versioning:  https://semver.org/