# Shared UI Razor Class Library (RCL) Demo

This demo is showcasing how to use razor class library to share same UI amongst different web projects (MVC, Razor Pages, Blazor Server and Blazor WASM). A lot has changed since initial demo (using .NET5) and use of embedded resources to serve static files from RCL. This updated approach works better with WASM as well and is much simpler to setup. 

Steps to add it to your project:
1. Add **Razor Class Library** to your solution:
`dotnet new razorclasslib --name Demo.SharedUI`
2. Add your static files to `wwwroot` folder of `RCL`
    - Cleanup `wwwroot` folder in any of your web projects where you want to use shared UI RCL. Keep what is specific to that project.
   
3. Project reference your newly created `RCL` in any of the web projects
    ``` c#
    <ItemGroup>
      <ProjectReference Include="..\Demo.SharedUI\Demo.SharedUI.csproj" />
    </ItemGroup>
    ```
   
4. Update `Pages/_Layout.cshtml` and references static css and js files
    ``` html
    <head>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
        <base href="~/"/>
        <link rel="stylesheet" href="_content/Demo.SharedUI/Theme/lib/bootstrap/dist/css/bootstrap.min.css"/> @* Add this line *@
        <link href="_content/Demo.SharedUI/Theme/css/site.css" rel="stylesheet" asp-append-version="true"/> @* Add this line *@
        <link href="Demo.Blazor.styles.css" rel="stylesheet"/>
        <component type="typeof(HeadOutlet)" render-mode="ServerPrerendered"/>
    </head>
   ```
   
5. For `Blazor Server` add the following 3 lines to `Program.cs` 
   ``` c#
   var builder = WebApplication.CreateBuilder(args);
    
   builder.WebHost.UseWebRoot("wwwroot"); // Add this line
   builder.WebHost.UseStaticWebAssets(); // Add this line
   
   ...
   
   var app = builder.Build();
   
   ...
   
   app.UseStaticFiles(); // Add this line
   
   ...
   ```