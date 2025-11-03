using Microsoft.EntityFrameworkCore;
using SmartStoreModels.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.Data
{
    public static class SeedData
    {

        public static async Task SeedCategory(AppDbContext _db)
        {
            if(!await _db.categories.AnyAsync())
            {
               await _db.categories.AddRangeAsync(
                    new Category { Name="Head Phone",Description="Head Phone",DateCreatedOn=DateTime.Now},
                    new Category { Name="Laptop",Description="Laptop",DateCreatedOn=DateTime.Now},
                    new Category { Name="Smart Watch",Description= "Smart Watch", DateCreatedOn=DateTime.Now},
                    new Category { Name="Phone",Description= "Phone", DateCreatedOn=DateTime.Now}
               );
            }

           await _db.SaveChangesAsync();
        }
    }
}
