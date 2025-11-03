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
    public interface IUserService
    {
        string GetUserId();
        string GetUserName();

        UserInfo GetUserInfo(string userId);
        Task<bool> IsValidUser(string email);

        Task<string> GetLoggedUserName(string email);


    }
}
