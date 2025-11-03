using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SmartStoreModels.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace SmartStoreData.Data
{
    public static class ExtensionMethods
    {
        public static async Task<string>GetUserId(UserManager<IdentityUser> um,IHttpContextAccessor hc)
        {
            var user=hc.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user == null)
            {
                var userId = await um.GetUserAsync(hc.HttpContext.User);
                user = userId?.Id;
            }
            return user;
        }

        public static async void SaveCommonFields(this AppDbContext db,UserManager<IdentityUser> um,IHttpContextAccessor hc)
        {
            var userId=await GetUserId(um, hc);
            IEnumerable<BaseModel> insert=db.ChangeTracker.Entries().
                Where(x=>x.State==EntityState.Added).Select(x=>x.Entity).OfType<BaseModel>().ToList();

            IEnumerable<BaseModel> update = db.ChangeTracker.Entries().
                Where(x => x.State == EntityState.Modified).Select(x => x.Entity).OfType<BaseModel>().ToList();
            foreach(var item in insert)
            {
                item.CreatedOn= DateTime.Now;
                item.CreatedBy = userId;

            }
            foreach(var item in update)
            {
                item.UpdatedOn= DateTime.Now;
                item.UpdatedBy=userId;
                item.CreatedOn= DateTime.Now;
                item.CreatedBy=userId;
            }
        }
    }
}
