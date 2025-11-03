using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _ui;
        private readonly IHttpContextAccessor _hc;
        private readonly IConfiguration _config;

        public ICategoryRepository cateogry { get; set; }

        public IProductRepository prodt { get; set; }

        public ICartRepository cart { get; set; }

        public ISummaryRepository summary { get; set; }
        public IUserRepository user { get; set; }

        public IOrderRepository order { get; set; }

        public IOrderSummaryRepository orderSummary { get;  }

        public UnitOfWork(AppDbContext db,UserManager<IdentityUser>ui,IHttpContextAccessor hc, IOrderSummaryRepository ordersummary)
        {
            _db=db;
            _ui=ui;
            _hc=hc;
           
            cateogry = new CategoryRepository(_db);
            prodt = new ProductRepository(_db);
            cart= new CartRepository(_db);
            summary=new SummaryRepository(_db);
            user=new UserRepository(_db);
            order=new OrderRepository(_db);
            orderSummary = ordersummary;

        }

        

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Save()
        {
             _db.SaveCommonFields(_ui,_hc);
            await _db.SaveChangesAsync();
        }
    }
}
