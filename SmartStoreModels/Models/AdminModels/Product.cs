using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SmartStoreModels.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.AdminModels
{
    public class Product:BaseModel
    {
        [Required]
        public string Name { get;set; }

        [Required]

        public string Description { get;set; }

        [Required]
        public decimal HighPrice {  get; set; }

        [Required]

        public decimal LowPrice { get; set; }

        [ValidateNever]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category category { get;set; }

        [ValidateNever]
        public string ProductImage { get; set; }
        [NotMapped]
        public IFormFile FormFileImage {  get; set; }
        [Required]
        public decimal Ratings { get; set; }
        
        public bool isValid = true;
    }
}
