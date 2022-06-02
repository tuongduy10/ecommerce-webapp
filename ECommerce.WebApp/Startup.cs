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

namespace ECommerce.WebApp
{
    public class Startup
    {
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
                    option.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                    option.AddPolicy("Seller", policy => policy.RequireRole("Admin", "Seller"));
                    option.AddPolicy("Buyer", policy => policy.RequireRole("Admin", "Seller", "Buyer"));
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

            services.AddMvc();
            // Services
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

            // Shop
            services.AddTransient<IShopService, ShopService>();

            //services
            //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>{
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = false,
            //            ValidateAudience = false,

            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey:Key"])),
            //            ClockSkew = TimeSpan.Zero
            //        };
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseCookiePolicy();

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
        }
    }
}
