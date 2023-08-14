using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.SubCategory;
using ECommerce.Application.Services.Category;
using ECommerce.Application.Services.Configurations;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Discount;
using ECommerce.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ECommerce.Application.Services.FilterProduct;
using ECommerce.Application.Services.User;
using ECommerce.Application.Services.Bank;
using ECommerce.Application.Services.Shop;
using ECommerce.Application.Services.Rate;
using ECommerce.Application.Services.Role;
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
using ECommerce.Application.Services.Product_v2;
using ECommerce.Application.Services.User_v2;
using Microsoft.Extensions.Logging;
using ECommerce.Data.Models;
using ECommerce.Application.Services.Chat;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

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
            services.AddDbContext<ECommerceContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ECommerceDB")));
            services.AddControllersWithViews();
            services
                .AddAuthorization(option =>
                {
                    option.AddPolicy("Admin", policy => {
                        policy.RequireRole("Admin");
                    });
                    option.AddPolicy("Seller", policy => {
                        policy.RequireRole("Admin", "Seller");
                    });
                    option.AddPolicy("Buyer", policy => {
                        policy.RequireRole("Admin", "Seller", "Buyer");
                    });
                })
                .AddAuthentication(option => option.DefaultAuthenticateScheme = "ClientAuth")
                .AddCookie("ClientAuth", option => {
                    option.AccessDeniedPath = "/Account/AccessDenied";
                    option.LoginPath = "/Account/SignIn";
                    option.Cookie.Name = "_clientcookie";
                    option.ExpireTimeSpan = TimeSpan.FromDays(30);
                    option.Cookie.MaxAge = option.ExpireTimeSpan;
                })
                .AddCookie("AdminAuth", option => {
                    option.AccessDeniedPath = "/Account/AccessDenied";
                    option.LoginPath = "/Admin/SignIn";
                    option.Cookie.Name = "_admincookie";
                    option.ExpireTimeSpan = TimeSpan.FromHours(4);
                    option.Cookie.MaxAge = option.ExpireTimeSpan;
                });
            services.AddCors(options =>
            {
                options.AddPolicy(myCorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44330", "http://localhost:3000", "https://hihichi.com")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddControllers();
            services.AddSignalR();
            services.AddHttpContextAccessor();
            services.AddCors();


            /*
             * Business Services
             */
            // Website Configuration
            services.AddTransient<IConfigurationService, ConfigurationService>();
            services.AddTransient<IHeaderService, HeaderService>();
            services.AddTransient<IFooterService, FooterService>();
            // Website Data
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ISubCategoryService, SubCategoryService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IFilterProductService, FilterProductService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<IRateService, RateService>();
            services.AddTransient<IDiscountService, DiscountService>();
            // User
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            // Shop
            services.AddTransient<IShopService, ShopService>();

            // Services ver 2
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IProductService_v2,ProductService_v2>();
            services.AddScoped<IUserService_v2, UserService_v2>();
            services.AddScoped<IChatService, ChatService>();

            /*
             * Config Services
             */
            //services.AddScoped<GlobalActionFilter>();
            //services.AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>();
            //services.Configure<SecurityStampValidatorOptions>(option => option.ValidationInterval = TimeSpan.FromSeconds(10));
            //services.AddHttpContextAccessor();
            //services.AddScoped<IUserService>();

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
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithRedirects("/error/{0}");
                app.UseDeveloperExceptionPage();
                // app.UseHsts();
            }
            else
            {
                // app.UseExceptionHandler("/error");
                // app.UseStatusCodePagesWithRedirects("/error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseMiddleware<NoCacheMiddleware>();
            app.UseCors(myCorsPolicy); ;

            // same like app.UseMiddleware<NoCacheMiddleware>();
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
                await next();
            });

            app.UseEndpoints(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
                routes.MapHub<ClientHub>("/client-hub");
                routes.MapHub<CommonHub>("/common-hub");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "{controller=Admin}/{action=Index}/{id?}");
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
