using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartStoreModels.Models.AdminModels;
using SmartStoreModels.Models.CommonMethods;
using SmartStoreModels.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.Data
{
    public class AppDbContext:IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<Category> categories { get; set; }

        public DbSet<Product> products { get; set; }

        public DbSet<Cart> carts { get; set; }

        public DbSet<ApplicationUser> appUsers {  get; set; }

        public DbSet<Summary> summary { get; set; }

        public DbSet<Orders> orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.HighPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.LowPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>().Property(p=>p.Ratings).HasPrecision(2, 1);
            modelBuilder.Entity<Product>().HasOne(p => p.category).WithMany().
                HasForeignKey(p => p.CategoryId).HasPrincipalKey(c => c.Id);
            modelBuilder.Entity<Product>().Property(p => p.isValid).HasDefaultValue(true);

            modelBuilder.Entity<Cart>().Property(c => c.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Cart>().Property(c => c.TotalAmout).HasPrecision(18, 2);
            modelBuilder.Entity<Cart>().Property(c => c.isValid).HasDefaultValue(true);
            modelBuilder.Entity<Cart>().Property(c => c.isApproved).HasDefaultValue(false);

            modelBuilder.Entity<Summary>().Property(c=>c.isValid).HasDefaultValue(true);

            modelBuilder.Entity<Summary>().Property(c => c.isActive).HasDefaultValue(true);

            modelBuilder.Entity<Orders>().Property(c => c.isActive).HasDefaultValue(true);

            modelBuilder.Entity<ApplicationUser>().Property(c => c.isValid).HasDefaultValue(true);
            modelBuilder.Entity<ApplicationUser>().Property(c => c.isActive).HasDefaultValue(true);
        }
    }
}
