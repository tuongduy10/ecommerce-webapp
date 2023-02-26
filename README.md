# Document

## Technologies:
`.Net Core 3.1`
`Entity Framework 3.1`
`Cookie Authentication`
`Firebase Authentication`
`SignalR`

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

## Models

Set "ECommerce.Data" as a start up project before generating or updating models.
 
Generate model classes from database:

`Scaffold-DbContext 'Data Source=.\SQLEXPRESS;Initial Catalog=ECommerce;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer -ContextDir Context -OutputDir Models`

Updating model classes from database:

`Scaffold-DbContext 'Data Source=.\SQLEXPRESS;Initial Catalog=ECommerce;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer -ContextDir Context -OutputDir Models -force`

Config ECommerceContext in Context at "ECommerce.Data":
![image](https://user-images.githubusercontent.com/63220379/163608126-548f39c8-c2d1-47b5-8c3e-b05bfb8cebbd.png)

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
