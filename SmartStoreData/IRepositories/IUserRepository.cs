using Microsoft.AspNetCore.Identity;
using SmartStoreModels.Models.CommonMethods;
using SmartStoreModels.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.IRepositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task RemoveUser(string id);

        Task IncludeUser(string id);
        Task RemoveActive(string id);
        Task<List<ApplicationUser>> GetAllUser();
    }
}
