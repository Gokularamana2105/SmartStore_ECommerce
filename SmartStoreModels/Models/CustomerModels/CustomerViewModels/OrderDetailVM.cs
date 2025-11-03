using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.CustomerModels.CustomerViewModels
{
    public class OrderDetailVM
    {
        public List<Summary> summary {  get; set; }

        public Orders order {  get; set; }
    }
}
