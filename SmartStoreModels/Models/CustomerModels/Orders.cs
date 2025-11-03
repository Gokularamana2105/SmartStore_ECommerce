using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SmartStoreModels.Models.CommonMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.CustomerModels
{
    public class Orders
    {
        [Key]
        public int Id {  get; set; }
        [ValidateNever]
        public int OrderNumber {  get; set; }
        [ValidateNever]

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser applicationUser { get; set; }
        public string UserName {  get; set; }

        public string PhoneNumber {  get; set; }

        public string City {  get; set; }

        [ValidateNever]
        public decimal Total {  get; set; }
        [ValidateNever]

        public string Status {  get; set; }

        public DateTime OrderedDate { get; set; }

        public DateTime ShippingDate {  get; set; }

        public bool isValid {  get; set; }

        public bool isActive { get; set; }

    }
}
