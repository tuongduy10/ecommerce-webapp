using ECommerce.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ECommerce.WebApp.Middlewares;
using ECommerce.WebApp.Configs.Middlewares;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ECommerce.WebApp.Configs.AppSettings;
using ECommerce.WebApp.Utils;
using ECommerce.Application.Extensions;
using Microsoft.AspNetCore.Http;

namespace ECommerce.WebApp
{
    public class Startup
    {
        private readonly string myCorsPolicy = "_myCorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            string secretKey = Configuration["AppSettings:SecretKey"];
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            string connStr = EncryptHelper.DecryptString(Configuration.GetConnectionString("ECommerceDB"));

            services.AddDbContext<ECommerceContext>(options => options.UseSqlServer(connStr));
            services.AddControllersWithViews();

            services
                .AddAuthentication(option =>
                {
                    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(option => 
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy(myCorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44330", "http://localhost:3000", "https://hihichi.com", "http://192.168.1.10:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddControllers();
            services.AddHttpClient();
            //services.AddSignalR();
            services.AddHttpContextAccessor();

            services.Configure<AppSetting>(Configuration.GetSection("AppSettings"));

            // Services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransientServices();
            services.AddScopedServices();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "client/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMiddleware<CustomAuthMiddleware>();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseMiddleware<NoCacheMiddleware>();
            app.UseCors(myCorsPolicy); ;

            //app.UseEndpoints(routes =>
            //{
            //    routes.MapHub<ChatHub>("/chatHub");
            //    routes.MapHub<ClientHub>("/client-hub");
            //    routes.MapHub<CommonHub>("/common-hub");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client";
                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

        }
    }
}
