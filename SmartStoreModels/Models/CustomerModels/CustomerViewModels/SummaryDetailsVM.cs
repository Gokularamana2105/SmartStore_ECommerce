using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.CustomerModels.CustomerViewModels
{
    public class SummaryDetailsVM
    {
        public UserInfo users {  get; set; }

        public List<Summary> summary { get; set; }

        public Orders orders { get; set; }
    }
}
