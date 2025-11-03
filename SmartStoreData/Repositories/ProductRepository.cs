using Microsoft.EntityFrameworkCore;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.AdminModels;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db):base(db) 
        {
            _db = db;
        }

        public async Task RemoveProduct(Guid id)
        {
            var query=await _db.products.FindAsync(id);
            if (query != null)
            {
                query.isValid = false;
            }

        }

        public async  Task<List<Product>> GetAllProduct()
        {
            var query=_db.products.Include(x=>x.category).Where(x=>x.isValid==true).ToListAsync();
            return await query;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var query = _db.products.Include(x => x.category).FirstOrDefaultAsync(x => x.Id == id);
            return await query;
        }

        public async Task Update(Product product)
        {
            var obj = await _db.products.FirstOrDefaultAsync(x=>x.Id==product.Id);
            if (obj != null)
            {
                obj.Name= product.Name;
                obj.Description= product.Description;
                obj.CategoryId= product.CategoryId;
                obj.ProductImage=product.ProductImage;
                obj.HighPrice=product.HighPrice;
                obj.LowPrice=product.LowPrice;
                obj.Ratings=product.Ratings;
            }
            _db.products.Update(obj);
        }

       
        public async Task<List<Product>> GetAllProduct(Guid categoryId, Guid? skipRecord)
        {

            var query = _db.products.Include(c => c.category).OrderByDescending(x => x.Name);
            if (categoryId == Guid.Empty)
            {
                return await query.ToListAsync();
            }
            else
            {
                query = (IOrderedQueryable<Product>)query.Where(x => x.CategoryId == categoryId);
            }
            var product = await query.ToListAsync();

            if (skipRecord.HasValue)
            {
                var recordProduct = product.FirstOrDefault(x => x.Id == skipRecord.Value);
                if (recordProduct != null)
                {
                    product.Remove(recordProduct);
                }
            }
            return product;
        }
    }
}
