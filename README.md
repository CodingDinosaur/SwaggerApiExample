# OpenAPI 3 / Swagger API Example <!-- omit in toc -->

This repository contains an application that shows an example of creating a self-documenting HTTP-based API designed to be consumed by others unfamiliar with it.  Inside, several different approaches are taken in various methods, varying from simple methods with primitive types, to complex wire-types and mutable responses that can vary depending on the response code, and more.

This application can build, run, and serve its API for educational purposes, but it does not provide any useful functionality.  Everything just below the service layers are just silly things designed to be filler points for the API.

- [Basic Info](#basic-info)
- [Building &amp; Running](#building-amp-running)
    - [With Docker](#with-docker)
    - [With Visual Studio &amp; Node](#with-visual-studio-amp-node)
- [What's In Here](#whats-in-here)
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

# Basic Info
- Written in C# 8.0
- Hosting application is ASP.NET in .NET Core 3
- Contains a mix of ASP.NET Web API controllers, basic static files, and an Angular SPA
- Takes advantage of the new SPA tools in ASP.NET, including integration between the ASP.NET dev server and the Angular dev server
- Uses Swashbuckle to generate Swagger documentation and Swagger UI pages

# Building & Running

### With Docker
Simply run *buildAndRun.sh*, which will build and run the docker container for you:
```console
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

# What's In Here

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