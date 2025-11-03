using Microsoft.AspNetCore.Mvc;
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
    public class Summary:BaseModel
    {
        [ValidateNever]
        public string ProductName {  get; set; }
        [ValidateNever]
        public string UserName {  get; set; }
        [ValidateNever]
        public string Phone {  get; set; }
        [ValidateNever]
        public string Address { get; set; }
        [ValidateNever]
        public string City { get; set; }
        [ValidateNever]
        public string ProductImage {  get; set; }
        [ValidateNever]
        public decimal Price {  get; set; }
        [ValidateNever]
        public int count {  get; set; }
        [ValidateNever]
        public decimal Total {  get; set; }
       
        public bool isValid { get; set; }

        public bool isActive { get; set; }
        public Guid CartId {  get; set; }
        [ForeignKey("CartId")]
        public Cart cart { get; set; }
        [NotMapped]
        public string FormImage { get; set; }
        
       
    }
}
