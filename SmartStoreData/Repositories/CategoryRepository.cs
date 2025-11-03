using Microsoft.EntityFrameworkCore;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.Repositories
{
    public class CategoryRepository : GenericRepository<Category>,ICategoryRepository
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db):base(db)
        {
            _db = db;
        }
        public async Task Update(Category ct)
        {
            var obj=await _db.categories.FirstOrDefaultAsync(x=>x.Id==ct.Id);
            if(obj != null)
            {
                obj.Name = ct.Name;
                obj.Description = ct.Description;
                obj.DateCreatedOn = DateTime.Now;
            }
            //_db.categories.Update(ct);

        }
    }
}
