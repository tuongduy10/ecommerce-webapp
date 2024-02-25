using ECommerce.Application.BaseServices.Brand;
using ECommerce.Application.BaseServices.SubCategory;
using ECommerce.Application.BaseServices.Category;
using ECommerce.Application.BaseServices.Configurations;
using ECommerce.Application.BaseServices.Product;
using ECommerce.Application.BaseServices.Discount;
using ECommerce.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ECommerce.Application.BaseServices.FilterProduct;
using ECommerce.Application.BaseServices.User;
using ECommerce.Application.BaseServices.Bank;
using ECommerce.Application.BaseServices.Shop;
using ECommerce.Application.BaseServices.Rate;
using ECommerce.Application.BaseServices.Role;
using Microsoft.AspNetCore.Http;
using ECommerce.WebApp.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ECommerce.WebApp.Configs.ActionFilters;
using ECommerce.WebApp.Configs.Middlewares;
using ECommerce.Application.Services.Notification;
using ECommerce.WebApp.Configs.ActionFilters.HttpResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services.Comment;
using ECommerce.WebApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.User;
using Microsoft.Extensions.Logging;
using ECommerce.Data.Models;
using ECommerce.Application.Services.Chat;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ECommerce.WebApp.Configs.AppSettings;
using ECommerce.Application.Services.Inventory;
using ECommerce.Application.Services.Common;
using ECommerce.WebApp.Utils;

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
            var secretKey = Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            string connSecretKey = Configuration["AppSettings:ConnectionSecretKey"];
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
            services.AddSignalR();
            services.AddHttpContextAccessor();

            services.Configure<AppSetting>(Configuration.GetSection("AppSettings"));

            /*
             * Business Services
             */
            // Base Services
            services.AddTransient<IConfigurationService, ConfigurationService>();
            services.AddTransient<IHeaderService, HeaderService>();
            services.AddTransient<IFooterService, FooterService>();
            services.AddTransient<IProductBaseService, ProductBaseService>();
            services.AddTransient<ISubCategoryService, SubCategoryService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IFilterProductService, FilterProductService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<IRateService, RateService>();
            services.AddTransient<IDiscountService, DiscountService>();
            services.AddTransient<IUserBaseService, UserBaseService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IShopService, ShopService>();

            // Services
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IChatService, ChatService>();

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
