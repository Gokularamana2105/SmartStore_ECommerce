using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.CommonMethods
{
    public static class Messages
    {
        public static string CategoryMethod(string message)
        {
            var categoryMessage = new Dictionary<string, string>()
            {
                 {"Create","Category Saved Successfully"}  ,
                 { "Update", "Category Updated Successfully" },
                 { "Delete", "Category Deleted Successfully" }
            };

            string result = categoryMessage.TryGetValue(message, out var category) ? category : "Invalid Message";
            return result;
        }

        public static string ProductMethod(string message)
        {
            var productMessage = new Dictionary<string, string>()
            {
                {"Create","Product Saved Successfully"},
                {"Update","Product Update Successfully"},
                {"Delete","Product Deleted Successfully"},
            };

            string result = productMessage.TryGetValue(message, out var product) ? product : "Invalid Message";
            return result;
        }
    }
}
