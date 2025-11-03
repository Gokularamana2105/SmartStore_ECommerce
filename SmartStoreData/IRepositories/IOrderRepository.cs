using SmartStoreModels.Models.AdminModels;
using SmartStoreModels.Models.CustomerModels;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.IRepositories
{
    public interface IOrderRepository:IGenericRepository<Orders>
    {
        Task<Orders> GetOrderById(string id);

        Task<List<Orders>> Update(Orders orders);
        Task<Orders>GetOrderNumber(int orderNumber); 
        
        Task RemoveOrder(int  orderId);

        Task <List<Orders>>RemoveActive(string userId);

       
    }
}
