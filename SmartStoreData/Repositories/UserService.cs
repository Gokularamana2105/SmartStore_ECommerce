using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.CommonMethods;
using SmartStoreModels.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.Repositories
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _ca;


        private readonly string _con;

        public UserService(IHttpContextAccessor ca,IConfiguration config )
        {
            _ca = ca;

            _con = config.GetConnectionString("DefaultConnection");
        }
        public string GetUserId()
        {

            var user= _ca.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return  user;
        }

       

        public string GetUserName()
        {
            return _ca.HttpContext?.User?.Identity?.Name;
        }

        public UserInfo GetUserInfo(string userId)
        {
            using (var connection=new SqlConnection(_con))
            {
                var param = new DynamicParameters(); //using Dapper
                param.Add("@UserId", userId);

                return connection.QueryFirstOrDefault<UserInfo>(
                    "Proc_GetUserInfoById",  //procedureName
                    param,  //Parameters
                    commandType:System.Data.CommandType.StoredProcedure //CommandType
                    );
            }
        }

        public async Task<bool> IsValidUser(string email)
        {
            using (var connection  = new SqlConnection(_con))
            {
                await connection.OpenAsync();
                var param=new DynamicParameters();
                param.Add("@Email", email);
                var result = await connection.QuerySingleOrDefaultAsync<bool>(
                    "Proc_GetValidUser",
                    param,
                    commandType:System.Data.CommandType.StoredProcedure
                    );
                return result;
            }
        }

        public async Task<string> GetLoggedUserName(string email)
        {
            using (var con=new SqlConnection(_con))
            {
                await con.OpenAsync();
                var param=new DynamicParameters();
                param.Add("@Email", email);
                var result = await con.QueryFirstOrDefaultAsync<string>("Proc_GetUserName",
                    param,
                    commandType: System.Data.CommandType.StoredProcedure
                    );
                return result;
            }
        }
    }
}
