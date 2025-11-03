using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SmartStoreModels.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.AdminModels
{
    public class Category:BaseModel
    {
        [Required]
        public string Name { get;set; }

        [Required]
        public string Description { get;set; }

        [ValidateNever]
        public DateTime DateCreatedOn {  get;set; }
    }
}
