using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.AdminModels
{
    public class OrderSummary
    {
        public string MonthName { get; set; }
        public int Approved { get; set; }
        public int Pending { get; set; }
        public int Processing { get; set; }
        
    }
}
