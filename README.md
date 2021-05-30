# Shared UI Razor Class Library Demo

This demo is showcasing how to use razor class library to share same UI amongst different web projects regardless of the version of the .NET Core (current is .NET 5 as of updating this readme file).

Steps to add it to your project:
1. Add **Razor Class Library** to your solution:
`dotnet new razorclasslib --name Demo.SharedUI`
2. Add necessary packages using **Package Manager Console** <br>
`install-package Microsoft.AspNetCore.StaticFiles` <br>
`install-package Microsoft.Extensions.FileProviders.Embedded` 
3. Create `wwwroot` folder and add your static files
4. Create two new classes

      - SharedUIConfigureOptions.cs <br>

          ``` c#
          using System;
          using Microsoft.AspNetCore.Builder;
          using Microsoft.AspNetCore.Hosting;
          using Microsoft.AspNetCore.StaticFiles;
          using Microsoft.Extensions.FileProviders;
          using Microsoft.Extensions.Options;

          namespace Demo.SharedUI
          {
              internal class SharedUIConfigureOptions : IPostConfigureOptions<StaticFileOptions>
              {
                  private readonly IWebHostEnvironment _environment;

                  public SharedUIConfigureOptions(IWebHostEnvironment environment)
                  {
                      _environment = environment;
                  }

                  public void PostConfigure(string name, StaticFileOptions options)
                  {
                      name = name ?? throw new ArgumentNullException(nameof(name));
                      options = options ?? throw new ArgumentNullException(nameof(options));

                      // Basic initialization in case the options weren't initialized by any other component
                      options.ContentTypeProvider = options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
                      if (options.FileProvider == null && _environment.WebRootFileProvider == null)
                      {
                          throw new InvalidOperationException("Missing FileProvider.");
                      }

                      options.FileProvider = options.FileProvider ?? _environment.WebRootFileProvider;

                      // Add our provider
                      var filesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, "wwwroot");
                      options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
                  }
              }
          }
          ```

      - StartupHelpers.cs

          ``` c#
          using Microsoft.Extensions.DependencyInjection;

          namespace Demo.SharedUI
          {
              public static class StartupHelpers
              {
                  public static void AddSharedUI(this IServiceCollection services)
                  {
                      services.ConfigureOptions(typeof(SharedUIConfigureOptions));
                  }
              }
          }
          ```

5. Add the following to the **Demo.SharedUI.csproj** file
      - Add the following to the `<PropertyGroup>`  
        ``` c#
        <PropertyGroup>
          <GenerateEmbeddedFilesManifest>true</GenerateEmbeddedFilesManifest>
        </PropertyGroup>
        ``` 
      - Add the following `<ItemGroup>` <br>
        ``` c#
        <ItemGroup>
            <EmbeddedResource Include="wwwroot\**\*" />
        </ItemGroup>
        ```
 6. Project reference your newly created RCL in any of the web projects
  ``` c#
  <ItemGroup>
    <ProjectReference Include="..\Demo.SharedUI\Demo.SharedUI.csproj" />
  </ItemGroup>
  ```
 7. In the `ConfigureServices` method of `Startup.cs` in your web project 
  ``` c#
  public void ConfigureServices(IServiceCollection services)
  {
      // the rest of the code removed for brevity
      services.AddSharedUI(); 
  }
  ```
