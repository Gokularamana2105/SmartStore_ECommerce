using Microsoft.EntityFrameworkCore;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.Repositories
{
    public class CartRepository:GenericRepository<Cart>,ICartRepository
    {
        private readonly AppDbContext _db;
        public CartRepository(AppDbContext db):base(db)
        {
            _db = db;
        }

        public async Task<List<Cart>> ActiveApproved(string userId)
        {
            var query=await _db.carts.Where(x=>x.CreatedBy == userId).ToListAsync();
            foreach(var cart in query)
            {
                cart.isApproved = true;
            }
            return query;
        }

        public async Task<List<Cart>> GetAllCart()
        {
          return  await _db.carts.Where(x=>x.isValid==true && x.isApproved==false).ToListAsync();
        }

        public async Task<List<Cart>> GetCartById(string id)
        {
            var query=await _db.carts.Where(x=>x.CreatedBy==id && x.isValid==true && x.isApproved==false).ToListAsync();
            return query;
        }

        public async Task<int> GetCount(Expression<Func<Cart, bool>> filter = null)
        {
            var query = _db.carts.Where(x => x.isValid == true && x.isApproved == false);
            if (filter != null)
            {
                query= query.Where(filter);
            }
            return await query.CountAsync();
        }

        public async Task<int> GetCountById(string id)
        {
           return await _db.carts.Where(x=>x.CreatedBy==id && x.isValid==true && x.isApproved==false).CountAsync();
        }

        public async Task RemoveCart(Guid id)
        {
            var query=await _db.carts.FindAsync(id);
            if (query != null)
            {
                query.isValid = false;
            }

        }

        public async Task Update(Cart cart)
        {
            var obj = await _db.carts.FirstOrDefaultAsync(x => x.Id == cart.Id);
            if (obj != null)
            {
                obj.count = cart.count;
                obj.Price = cart.Price;
                obj.TotalAmout = cart.Price * cart.count;
            }

        }


    }
}
