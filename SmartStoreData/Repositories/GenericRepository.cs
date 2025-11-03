using Microsoft.EntityFrameworkCore;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace SmartStoreData.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _db;

        public DbSet<T> dbSet { get; private set; }
        public GenericRepository(AppDbContext db)
        {
            _db= db;
            dbSet = db.Set<T>();
        }
        public async Task Add(T entity)
        {
           await dbSet.AddAsync(entity);
        }

        public async Task<T> FindById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            var query= dbSet.ToListAsync();
            return await query;
        }

        public async Task<List<T>> GetAllByAsync(Expression<Func<T, bool>> filter)
        {
           var query= dbSet.Where(filter).ToListAsync();
            return await query;
        }

        public async Task<bool> isExist(Expression<Func<T, bool>> filter)
        {
          return await dbSet.Where(filter).AnyAsync();
          
        }

        public async Task<IEnumerable<T>> Query()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> filter)
        {
            var query = dbSet.Where(filter).ToListAsync();
            return await query;
        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public int Count(Expression<Func<T, bool>> filter=null)
        {
            if (filter != null)
            {
                return dbSet.Count(filter);
            }
          return dbSet.Count();
        }

        public async Task<T> GetRowById(string id)
        {
            var query = await dbSet.FindAsync(id);
            return query;
        }

        public  async Task RemoveAll(Expression<Func<T, bool>> filter)
        {
            var entity=await dbSet.Where(filter).ToListAsync();
            if (entity.Any())
            {
                dbSet.RemoveRange(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<T> FindId(int id)
        {
            var query=dbSet.FindAsync(id);
            return await query;
        }
    }
}
