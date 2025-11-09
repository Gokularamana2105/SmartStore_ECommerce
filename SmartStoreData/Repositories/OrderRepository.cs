using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.AdminModels;
using SmartStoreModels.Models.CustomerModels;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SmartStoreData.Repositories
{
    public class OrderRepository:GenericRepository<Orders>,IOrderRepository
    {
        private readonly AppDbContext _db;
        private readonly string _con;
        public OrderRepository(AppDbContext db):base(db)
        {
            _db = db;
           
        }

      

        public async Task<Orders> GetOrderById(string id)
        {
            var orders = await _db.orders.Where(x=>x.UserId==id).OrderByDescending(x=>x.Id).FirstOrDefaultAsync();
            return orders;

        }

        public async Task<Orders> GetOrderNumber(int orderNumber)
        {
            var orders=await _db.orders.Where(x=>x.OrderNumber==orderNumber).OrderByDescending(x=>x.Id).FirstOrDefaultAsync();
            return orders;
        }

        public async Task<List<Orders>> RemoveActive(string userId)
        {
            var query=await _db.orders.Where(x=>x.UserId== userId && x.isActive==true && (x.Status!= "Approved" || x.Status!="Processing")).ToListAsync();
            foreach (var order in query)
            {
                order.isActive = false;
            }
            return query;
        }

        public async Task RemoveOrder(int orderId)
        {
            //var query =await _db.orders.FindAsync(orderId);
            //if(query != null)
            //{
            //    query.isValid = false;
            //    query.isActive = false;
            //}

            if(await _db.orders.FindAsync(orderId) is { } query)
            {
               query.isValid= query.isActive = false;

            }
        }


        public async Task<List<Orders>> Update(Orders orders)
        {
            var lastId=await _db.orders.Where(x=>x.UserId == orders.UserId && (x.Status!="Approved"|| x.Status!="Processing")).
                        OrderByDescending(x=>x.Id).FirstOrDefaultAsync();
            if(lastId != null)
            {
                lastId.OrderNumber = orders.OrderNumber;
                lastId.OrderedDate = orders.OrderedDate == DateTime.MinValue ? DateTime.Now : orders.OrderedDate;
                lastId.ShippingDate = orders.ShippingDate;
                lastId.Status = orders.Status;
            }
            var obj = await _db.orders.Where(x => x.UserId == orders.UserId ).ToListAsync();
            foreach (var query in obj)
            {
                if (query.Id != lastId.Id)
                {
                    query.isValid = query.isActive = false;
                }
                
            }
            return obj;
        }

       
    }
}
