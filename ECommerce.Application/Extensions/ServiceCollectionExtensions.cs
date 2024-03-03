using ECommerce.Application.BaseServices.Bank;
using ECommerce.Application.BaseServices.Brand;
using ECommerce.Application.BaseServices.Category;
using ECommerce.Application.BaseServices.Configurations;
using ECommerce.Application.BaseServices.Discount;
using ECommerce.Application.BaseServices.FilterProduct;
using ECommerce.Application.BaseServices.Product;
using ECommerce.Application.BaseServices.Rate;
using ECommerce.Application.BaseServices.Role;
using ECommerce.Application.BaseServices.Shop;
using ECommerce.Application.BaseServices.SubCategory;
using ECommerce.Application.BaseServices.User;
using ECommerce.Application.Services.Chat;
using ECommerce.Application.Services.Comment;
using ECommerce.Application.Services.Common;
using ECommerce.Application.Services.Inventory;
using ECommerce.Application.Services.Notification;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTransientServices(this IServiceCollection services)
        {
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
        }
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IChatService, ChatService>();
        }
    }
}
