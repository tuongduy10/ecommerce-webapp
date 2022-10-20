using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ECommerce.Data.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

#nullable disable

namespace ECommerce.Data.Context
{
    public partial class ECommerceContext : DbContext
    {
        public ECommerceContext()
        {
        }

        public ECommerceContext(DbContextOptions<ECommerceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Models.Attribute> Attributes { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<BrandCategory> BrandCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Header> Headers { get; set; }
        public virtual DbSet<Interest> Interests { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<OptionValue> OptionValues { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductOptionValue> ProductOptionValues { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<ProductUserImage> ProductUserImages { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<RatingImage> RatingImages { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<ShopBank> ShopBanks { get; set; }
        public virtual DbSet<ShopBrand> ShopBrands { get; set; }
        public virtual DbSet<SizeGuide> SizeGuides { get; set; }
        public virtual DbSet<Social> Socials { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<SubCategoryAttribute> SubCategoryAttributes { get; set; }
        public virtual DbSet<SubCategoryOption> SubCategoryOptions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Attribute>(entity =>
            {
                entity.ToTable("Attribute");

                entity.Property(e => e.AttributeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("Bank");

                entity.Property(e => e.BankAccountName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BankAccountNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BankImage).HasColumnType("text");

                entity.Property(e => e.BankName).HasMaxLength(100);
            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.ToTable("Banner");

                entity.Property(e => e.BannerPath)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Blog");

                entity.Property(e => e.BlogContent).HasColumnType("ntext");

                entity.Property(e => e.BlogTitle).HasMaxLength(100);
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.BrandImagePath)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.Brands)
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("FK_Brand_Discount");
            });

            modelBuilder.Entity<BrandCategory>(entity =>
            {
                entity.HasKey(e => new { e.BrandId, e.CategoryId });

                entity.ToTable("BrandCategory");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.BrandCategories)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BrandCategory_Brand");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.BrandCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BrandCategory_Category");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.AddressUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FacebookUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FaviconPath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LogoPath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Owner).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.WebsiteName).HasMaxLength(100);
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.DiscountCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ForBrand).HasColumnName("forBrand");

                entity.Property(e => e.ForGlobal).HasColumnName("forGlobal");

                entity.Property(e => e.ForShop).HasColumnName("forShop");

                entity.Property(e => e.IsPercent).HasColumnName("isPercent");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Header>(entity =>
            {
                entity.ToTable("Header");

                entity.Property(e => e.HeaderName).HasMaxLength(50);

                entity.Property(e => e.HeaderUrl)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Interest>(entity =>
            {
                entity.HasKey(e => new { e.RateId, e.UserId });

                entity.ToTable("Interest");

                entity.HasOne(d => d.Rate)
                    .WithMany(p => p.Interests)
                    .HasForeignKey(d => d.RateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Interest_Rate");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Interests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Interest_User");
            });

            modelBuilder.Entity<Option>(entity =>
            {
                entity.ToTable("Option");

                entity.Property(e => e.OptionName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OptionValue>(entity =>
            {
                entity.ToTable("OptionValue");

                entity.Property(e => e.IsBaseValue).HasColumnName("isBaseValue");

                entity.Property(e => e.OptionValueName).HasMaxLength(50);

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.OptionValues)
                    .HasForeignKey(d => d.OptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OptionValue_Option");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Address).HasColumnType("text");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DiscountCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Recipient).HasMaxLength(100);

                entity.Property(e => e.Temporary).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK_Order_PaymentMethod");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Order_User");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderDetailId, e.OrderId, e.ShopId });

                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId).ValueGeneratedOnAdd();

                entity.Property(e => e.AttributeValue).HasMaxLength(50);

                entity.Property(e => e.OptionValue)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PayForAdmin).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PriceOnSell).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductName).HasMaxLength(100);

                entity.Property(e => e.ShopName).HasMaxLength(100);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.VerifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.PaymentMethod1)
                    .HasMaxLength(50)
                    .HasColumnName("PaymentMethod");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Insurance).HasMaxLength(30);

                entity.Property(e => e.Link).HasColumnType("ntext");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.PPC)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("PPC");

                entity.Property(e => e.ProductAddedDate).HasColumnType("datetime");

                entity.Property(e => e.ProductCode).HasMaxLength(50);

                entity.Property(e => e.ProductDescription).HasColumnType("ntext");

                entity.Property(e => e.ProductDescriptionMobile).HasColumnType("ntext");

                entity.Property(e => e.ProductImportDate).HasColumnType("date");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SizeGuide).HasColumnType("ntext");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Brand");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Shop");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_SubCategory");
            });

            modelBuilder.Entity<ProductAttribute>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.AttributeId });

                entity.ToTable("ProductAttribute");

                entity.Property(e => e.Value).HasMaxLength(50);

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.ProductAttributes)
                    .HasForeignKey(d => d.AttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductAttribute_Attribute");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductAttributes)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductAttribute_Product");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("ProductImage");

                entity.Property(e => e.ProductImagePath)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImage_Product");
            });

            modelBuilder.Entity<ProductOptionValue>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.OptionValueId });

                entity.ToTable("ProductOptionValue");

                entity.HasOne(d => d.OptionValue)
                    .WithMany(p => p.ProductOptionValues)
                    .HasForeignKey(d => d.OptionValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOptionValue_OptionValue");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOptionValues)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOptionValue_Product");
            });

            modelBuilder.Entity<ProductPrice>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ProductTypeId });

                entity.ToTable("ProductPrice");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PriceOnSell).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPrices)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductPrice_Product");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.ProductPrices)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductPrice_ProductType");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.ToTable("ProductType");

                entity.Property(e => e.ProductTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<ProductUserImage>(entity =>
            {
                entity.ToTable("ProductUserImage");

                entity.Property(e => e.ProductUserImagePath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductUserImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductUserImage_Product");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.ToTable("Rate");

                entity.Property(e => e.Comment).HasColumnType("ntext");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.HtmlPosition)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Rate_Product");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RateUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Rate_User");

                entity.HasOne(d => d.UserReplied)
                    .WithMany(p => p.RateUserReplieds)
                    .HasForeignKey(d => d.UserRepliedId)
                    .HasConstraintName("FK_Rate_UserReplied");
            });

            modelBuilder.Entity<RatingImage>(entity =>
            {
                entity.ToTable("RatingImage");

                entity.Property(e => e.RatingImagePath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Rate)
                    .WithMany(p => p.RatingImages)
                    .HasForeignKey(d => d.RateId)
                    .HasConstraintName("FK_RatingImage_Rate");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.ToTable("Shop");

                entity.Property(e => e.ShopAddress).HasMaxLength(100);

                entity.Property(e => e.ShopCityCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShopDistrictCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShopJoinDate).HasColumnType("datetime");

                entity.Property(e => e.ShopMail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShopName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ShopPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShopWardCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.Shops)
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("FK_Shop_Discount");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Shops)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Shop_User");
            });

            modelBuilder.Entity<ShopBank>(entity =>
            {
                entity.ToTable("ShopBank");

                entity.Property(e => e.ShopAccountName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShopAccountNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShopBankName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.ShopBanks)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShopBank_Shop");
            });

            modelBuilder.Entity<ShopBrand>(entity =>
            {
                entity.HasKey(e => new { e.ShopId, e.BrandId });

                entity.ToTable("ShopBrand");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.ShopBrands)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShopBrand_Brand");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.ShopBrands)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShopBrand_Shop");
            });

            modelBuilder.Entity<SizeGuide>(entity =>
            {
                entity.ToTable("SizeGuide");

                entity.Property(e => e.IsBaseSizeGuide).HasColumnName("isBaseSizeGuide");

                entity.Property(e => e.SizeContent).HasColumnType("ntext");
            });

            modelBuilder.Entity<Social>(entity =>
            {
                entity.ToTable("Social");

                entity.Property(e => e.Icon)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SocialName).HasMaxLength(100);

                entity.Property(e => e.SocialUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("SubCategory");

                entity.Property(e => e.SubCategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubCategory_Category");

                entity.HasOne(d => d.SizeGuide)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.SizeGuideId)
                    .HasConstraintName("FK_SubCategory_SizeGuide");
            });

            modelBuilder.Entity<SubCategoryAttribute>(entity =>
            {
                entity.HasKey(e => new { e.AttributeId, e.SubCategoryId });

                entity.ToTable("SubCategoryAttribute");

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.SubCategoryAttributes)
                    .HasForeignKey(d => d.AttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubCategoryAttribute_Attribute");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.SubCategoryAttributes)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubCategoryAttribute_SubCategory");
            });

            modelBuilder.Entity<SubCategoryOption>(entity =>
            {
                entity.HasKey(e => new { e.SubCategoryId, e.OptionId });

                entity.ToTable("SubCategoryOption");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.SubCategoryOptions)
                    .HasForeignKey(d => d.OptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubCategoryOption_Option");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.SubCategoryOptions)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubCategoryOption_SubCategory");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.IsSystemAccount).HasColumnName("isSystemAccount");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserAddress).HasMaxLength(100);

                entity.Property(e => e.UserCityCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserDistrictCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserFullName).HasMaxLength(100);

                entity.Property(e => e.UserJoinDate).HasColumnType("datetime");

                entity.Property(e => e.UserMail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserWardCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("UserRole");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
