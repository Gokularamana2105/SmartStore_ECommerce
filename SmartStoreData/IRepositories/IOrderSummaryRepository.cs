using SmartStoreModels.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.IRepositories
{
    public interface IOrderSummaryRepository
    {
        Task<List<OrderSummary>> GetMontlyOrders(string fromDate, string toDate);
    }
}
