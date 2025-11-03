using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.AdminModels.AdminViewModels
{
    public class DashboardVM
    {
        public int TotalOrders { get; set; }
        public int ApprovedOrders { get; set; }
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        

    }
}
