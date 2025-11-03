using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.AdminModels.AdminViewModels
{
    public class ProductVM
    {
        public List<Product> products {  get; set; }

        public Product prd { get; set; }

        public IEnumerable<SelectListItem> categories { get; set; }


    }
}
