using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository
{
    public class AppDbContext : DbContext
    {
        // DbContextOptions ile veritabanı bağlantısını appsettings.json dosyasındaki ConnectionStrings içerisindeki DefaultConnection ile yaparız.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

    }
}