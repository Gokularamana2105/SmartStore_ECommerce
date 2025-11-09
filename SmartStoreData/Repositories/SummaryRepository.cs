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
    public class SummaryRepository :GenericRepository<Summary>, ISummaryRepository
    {
        private readonly AppDbContext _db;
        public SummaryRepository(AppDbContext db):base(db) 
        {
            _db = db;
        }
        public async Task Update(Summary summary)
        {
            var obj =await _db.summary.FirstOrDefaultAsync(x => x.Id == summary.Id);
            if (obj != null)
            {

                obj.ProductImage = summary.ProductImage;
                obj.count=summary.count;
                obj.Total=summary.Total;
                obj.Price=summary.Price;
            }
        }

        public async Task<List<Summary>> GetSummaryById(string id)
        {

            //var query = await _db.summary.Include(x=>x.cart).Where(x => x.CreatedBy == id && x.cart.isValid==true && x.isActive==true).GroupBy(x => new { x.ProductName, x.Price })
            //    .Select(g => g.OrderByDescending(x => x.CreatedOn).FirstOrDefault()).ToListAsync();

            var query = await _db.summary.Include(x => x.cart).Where(x => x.CreatedBy == id && x.cart.isValid == true && x.isActive == true).OrderByDescending(x=>x.CreatedOn).ToListAsync();
            return query;
        }

        public async Task<List<Summary>> RemoveAllSummary(string userId)
        {
            var query = await _db.summary.Where(x => x.CreatedBy == userId && x.isActive).ToListAsync();
            foreach(var summary in query)
            {
                summary.isActive = false;
            }
            return query;
        }
    }
}
