using SmartStoreModels.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.IRepositories
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        Task Update(Category ct);


    }
}
