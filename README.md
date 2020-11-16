![Deploy](https://github.com/renanmarcos/authorization-for-ocelot/workflows/Deploy/badge.svg)

**AuthorizationForOcelot** allows you to implement a custom authorizer service with **[Ocelot](https://github.com/ThreeMammals/Ocelot)** configuration file style. 

This package is inspired and designed to use with **[SwaggerForOcelot](https://github.com/Burgyn/MMLib.SwaggerForOcelot)**, so for example you can use splitted into folder ocelot files.

## Getting Started

1. Configure Ocelot Gateway.
   > Follow the [Ocelot documentation](https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html).
   > Optionally, configure [SwaggerForOcelot](https://github.com/Burgyn/MMLib.SwaggerForOcelot/blob/master/README.md)
2. Install Nuget package into yout ASP.NET Core Ocelot project.
   > dotnet add package AuthorizationForOcelot
3. Configure allowed scopes for authorizing AuthorizationForOcelot in `ocelot.json`.

```Json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5001
        }
      ],
      "UpstreamPathTemplate": "/customers/{everything}",
      "UpstreamHttpMethod": [ "Post" ],
      "UserRoles": [ "admin", "uploader" ]
    },
    {
      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5001
        }
      ],
      "UpstreamPathTemplate": "/customers/{everything}",
      "UpstreamHttpMethod": [ "Get" ],
      "UserRoles": [ "user" ]
    },
    {
      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5002
        }
      ],
      "UpstreamPathTemplate": "/products/{everything}",
      "UpstreamHttpMethod": [ "Get", "Post" ],
      "UserRoles": [ "admin" ]
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://localhost:5000"
  }
}
```

   > `UserRoles` is the access level required for using the resource matching by Method, URI template, hosts and ports.

4. Create your custom authorize service implementing `AuthorizationForOcelot.Abstractions.IAuthorizerForOcelot`

```CSharp
public class MyCustomAuthorizerService : IAuthorizerForOcelot
{
    public IEnumerable<string> FetchUserRoles(HttpRequest httpRequest)
    {
        // httpRequest object has the access token passed to Ocelot gateway, you can get the Bearer token here for example.
        // Your logic must return an enumerable string of roles that user has access. Example:
        return new List<string>()
        {
            "operator",
            "accountant"
        };
    }
}
```

5. In the `ConfigureServices` method of `Startup.cs`, register the AuthorizationForOcelot with your custom implemented authorizer.

```CSharp
services.AddAuthorizationWithOcelot<MyCustomAuthorizerService>(Configuration);
services.AddOcelot().AddAuthorizationOcelotHandler();
```

6. In `ConfigureWebHostDefaults` method of `Program.cs`, insert the `AuthorizationForOcelot` configuration.

```CSharp
webBuilder.UseKestrel()
    .ConfigureAppConfiguration((hostingContext, config) => 
    {
        config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: false);
        config.AddOcelotWithAuthorization(hostingContext.HostingEnvironment);                            
    })
    .UseStartup<Startup>();
```
   > You can use options to define custom ocelot filename or folder. See `demo` folder of this repository to more examples.
