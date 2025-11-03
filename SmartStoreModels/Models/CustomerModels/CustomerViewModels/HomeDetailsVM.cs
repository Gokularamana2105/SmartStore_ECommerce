using SmartStoreModels.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.CustomerModels.CustomerViewModels
{
    public class HomeDetailsVM
    {
      public List<Product> product {  get; set; }

      public Product prodt { get; set; } 

      public Cart cart { get; set; }
    }
}
