using Microsoft.EntityFrameworkCore;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.CommonMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.Repositories
{
    public class UserRepository:GenericRepository<ApplicationUser>,IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db):base(db)
        {
            _db = db;

        }

        public async Task<List<ApplicationUser>> GetAllUser()
        {
            var query = await dbSet.Where(x => x.isValid == true).ToListAsync();
            return query;
        }

        public async Task IncludeUser(string id)
        {
            var query = await _db.appUsers.FindAsync(id);
            if (query != null)
            {
                query.isActive = true;
            }
        }

        public async Task RemoveActive(string id)
        {
            var query = await _db.appUsers.FindAsync(id);
            if (query != null)
            {
                query.isActive = false;
            }
        }

        public  async Task RemoveUser(string id)
        {
           var query=await _db.appUsers.FindAsync(id);
            if(query != null)
            {
                query.isValid = false;
            }
        }
    }
}
