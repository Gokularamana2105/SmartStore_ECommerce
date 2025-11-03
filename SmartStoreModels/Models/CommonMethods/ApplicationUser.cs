using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreModels.Models.CommonMethods
{
    public class ApplicationUser:IdentityUser
    {
        [ValidateNever]
        public string Name {  get; set; }

        [ValidateNever]
        public string street {  get; set; }

        [ValidateNever]
        public string city { get; set; }

        public bool isValid { get; set; }

        public bool isActive{ get; set; }

    }
}
