using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        // DbContextOptions ile veritabanı bağlantısını appsettings.json dosyasındaki ConnectionStrings içerisindeki DefaultConnection ile yaparız.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity entity)
                {

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.Now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                    }
                }

            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                if (entry.Entity is BaseEntity entity)
                {
                    entity.CreatedAt = DateTime.Now;

                }
            }
            return base.SaveChanges();
        }

    }
}