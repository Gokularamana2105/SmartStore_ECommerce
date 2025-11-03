using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.CustomerModels
{
    public class CheckoutRequest
    {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string City { get; set; }
            public string Phone { get; set; }
            public int OrderNumber { get; set; }
            public string Status { get; set; }

            public int Id {  get; set; }

            public List<Summary> Summary { get; set; }
        
    }
}
