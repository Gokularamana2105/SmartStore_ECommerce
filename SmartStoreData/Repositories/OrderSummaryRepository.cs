using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.Repositories
{
    public class OrderSummaryRepository : IOrderSummaryRepository
    {
        private readonly string _con;
        public OrderSummaryRepository( IConfiguration config)
        {
            _con = config.GetConnectionString("DefaultConnection");
        }
        public async Task<List<OrderSummary>> GetMontlyOrders(string fromDate, string toDate)
        {
            using (var connection = new SqlConnection(_con))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@FromDate", fromDate);
                param.Add("@ToDate", toDate);

                var result = await connection.QueryAsync<OrderSummary>(
                    "Proc_SSE_GetOrders",
                   param,
                    commandType: CommandType.StoredProcedure
                    );
                return result.ToList();
            }
        }
    }
}
