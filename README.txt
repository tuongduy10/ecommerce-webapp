Technologies: 
	+ .NET Core 5.0
	+ Enity Framework Core 5.0
Nuget Packages:
	1. ECommerceData
		+ Microft.EntityFrameworkCore.SqlServer (5.0.0).
		+ Microft.EntityFrameworkCore.Tools (5.0.0).
		+ Microft.EntityFrameworkCore.Design (5.0.0).
		+ Microft.Extensions.Configuration.FileExtensions (5.0.0).
		+ Microft.Extensions.Configuration.Json (5.0.0).
	2. ECommerceWebApp
		+ Microft.EntityFrameworkCore.SqlServer (5.0.0).
		+ Microft.EntityFrameworkCore.Tools (5.0.0).
		+ Microft.EntityFrameworkCore.Design (5.0.0).
		+ Microft.VisualStudio.Web.CodeGeneration.Design (5.0.0).
Generate models classes from database : 
	Scaffold-DbContext 'Data Source=.\SQLEXPRESS;Initial Catalog=ECommerce;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer -ContextDir Context -OutputDir Models

Update models classes when database changes:
	Scaffold-DbContext 'Data Source=.\SQLEXPRESS;Initial Catalog=ECommerce;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer -ContextDir Context -OutputDir Models -force
	
Config ECommerceContext:
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