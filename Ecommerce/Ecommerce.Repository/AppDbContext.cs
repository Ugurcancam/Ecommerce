using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        // DbContextOptions ile veritabanı bağlantısını appsettings.json dosyasındaki ConnectionStrings içerisindeki DefaultConnection ile yaparız.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<AltCategory> AltCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Favori - User ilişkisi
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);
            // Favori - Product ilişkisi
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ProductId);

            // AppUser - Basket ilişkisi
            modelBuilder.Entity<Basket>()
                .HasOne(b => b.User)
                .WithMany(u => u.Baskets)
                .HasForeignKey(b => b.UserId);

            // Product - BasketItem ilişkisi
            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Product)
                .WithMany(p => p.BasketItems)
                .HasForeignKey(bi => bi.ProductId);

            // Basket - BasketItem ilişkisi
            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Basket)
                .WithMany(b => b.BasketItems)
                .HasForeignKey(bi => bi.BasketId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Product productEntity)
                {
                    // Product'den türeyen entity'lerde CreatedAt property'si eklenirken şuanki zamanı atıyoruz.
                    if (entry.State == EntityState.Added)
                    {
                        productEntity.CreatedAt = DateTime.Now;
                    }
                    // Product'den türeyen entity'lerde CreatedAt property'si güncellenirken değişmemesi için CreatedAt property'si modifiye edilmez.
                    else if (entry.State == EntityState.Modified)
                    {
                        Entry(productEntity).Property(x => x.CreatedAt).IsModified = false;
                    }

                    // Product entity'si eklenirken IsActive true olacak
                    productEntity.IsActive = true;
                    productEntity.InStock = true;
                    if (productEntity.Stock == 0)
                    {
                        productEntity.InStock = false;
                    }
                }
                else if (entry.Entity is Blog blogEntity)
                {
                    // Blog'dan türeyen entity'lerde CreatedAt property'si eklenirken şuanki zamanı atıyoruz.
                    if (entry.State == EntityState.Added)
                    {
                        blogEntity.CreateDate = DateTime.Now;
                        blogEntity.Status = true;
                    }
                    // Blog'dan türeyen entity'lerde CreatedAt property'si güncellenirken değişmemesi için CreatedAt property'si modifiye edilmez.
                    else if (entry.State == EntityState.Modified)
                    {
                        Entry(blogEntity).Property(x => x.CreateDate).IsModified = false;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                if (entry.Entity is Product entity)
                {
                    entity.CreatedAt = DateTime.Now;

                }
            }
            return base.SaveChanges();
        }

    }
}