using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SmartStoreModels.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.CustomerModels
{
    public class Cart:BaseModel
    {
        [ValidateNever]
        public string Name {  get; set; }

        [ValidateNever]
        public string Description { get; set; }
        [ValidateNever]
        public string ProductImage {  get; set; }
        [ValidateNever]
        public decimal Price {  get; set; }
        [ValidateNever]
        public int count {  get; set; }
        [ValidateNever]
        public decimal TotalAmout {  get; set; }
        [NotMapped]

        public string FormImage { get; set; }
        [ValidateNever]
        public bool isValid { get; set; }

        public bool isApproved { get; set; }
    }
}
