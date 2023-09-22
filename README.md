# Document

## Technologies:
`.Net Core 3.1`
`Entity Framework 3.1`
`Cookie Authentication`
`Firebase Authentication`
`SignalR`
`React`
`Redux Toolkit`
`JWT`
`TailwindCSS`

## Nuget Packages

In "ECommerce.Data":

`Microft.EntityFrameworkCore.SqlServer (3.1.0)`
`Microft.EntityFrameworkCore.Tools (3.1.0)`
`Microft.EntityFrameworkCore.Design (3.1.0)`
`Microft.Extensions.Configuration.FileExtensions (3.1.0)`
`Microft.Extensions.Configuration.Json (3.1.0)`

In ECommerce.WebApp too:

`Microft.EntityFrameworkCore.SqlServer (3.1.0)`
`Microft.EntityFrameworkCore.Tools (3.1.0)`
`Microft.EntityFrameworkCore.Design (3.1.0)`
`Microft.EntityFrameworkCore.Design (3.1.0)`
`Microft.VisualStudio.Web.CodeGeneration.Design (3.1.0)`

## Entities

Set "ECommerce.Data" as a start up project before generating or updating models (Currently using DB First).
 
Generate model classes from database:

`Scaffold-DbContext 'Data Source=.\SQLEXPRESS;Initial Catalog=ECommerce;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer -ContextDir Context -OutputDir Models`

Updating model classes from database:

`Scaffold-DbContext 'Data Source=.\SQLEXPRESS;Initial Catalog=ECommerce;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer -ContextDir Context -OutputDir Models -force`

Config ECommerceContext in Context at "ECommerce.Data":
 ```
 // Get connection string from appsettings.json
 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 {
   if (!optionsBuilder.IsConfigured)
     {
       IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
       optionsBuilder.UseSqlServer(configuration.GetConnectionString("ECommerceDB"));
     }
 }
```

# Get started

## Folder structure

ecommerce-webapp
```
├── ECommerce.Application
│   ├── Common
│   ├── Constants
│   ├── Helpers
│   ├── Repositories
│   ├── Services
│   └── .csproj
├── ECommerce.Data
│   ├── Context
│   ├── Models // Entities
│   ├── appsetting.json // do not change
│   └── .csproj
├── ECommerce.WebApp
│   ├── APIs
│   ├── client // React source project (see the client structure below)
│   ├── Configs
│   ├── Hubs
│   ├── Models // DTO or ViewModel
│   ├── Properties
│   │   └── launchSettings.json // launch url
│   ├── Utils
│   ├── ViewComponents
│   ├── Views
│   ├── wwwroot // project assets (images, css, js, ...)
│   ├── appsettings.json
│   │   ├── appsettings.Development.json // can be changed when under development
│   │   └── appsettings.Production.json // do not change
│   └── Startup.cs // Config service, endpoint, context. auth
├── .gitignore
├── READTME.md
└── READTME.txt
```

client
```
├── public
├── src
│   ├── _configs
│   │   ├── enviroment.config.ts
│   │   ├── global.config.ts
│   │   └── web.config.ts
│   ├── _cores
│   │   ├── _api
│   │   ├── _enums
│   │   ├── _interfaces
│   │   ├── _routes
│   │   ├── _services
│   │   └── _store
│   ├── _pages
│   │   ├── admin
│   │   │   └── components
│   │   ├── example
│   │   │   ├── _components
│   │   │   ├── _enums
│   │   │   ├── _interfaces
│   │   │   ├── _styles
│   │   │   └── .page.tsx
│   │   └── index.ts
│   ├── _shares
│   │   ├── _assets
│   │   ├── _components
│   │   ├── _helpers
│   │   └── _layouts
│   ├── App.tsx
│   └── index.tsx
├── node_modules
├── package.json
├── package-lock.json 
└── .gitignore
```
