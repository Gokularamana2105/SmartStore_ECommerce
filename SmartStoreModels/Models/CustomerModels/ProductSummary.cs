using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.CustomerModels
{
    public class ProductSummary
    {
        public string ProductName {  get; set; }

        public string ImageUrl {  get; set; }

        public int Count {  get; set; }

        public decimal Price {  get; set; }
    }
}
