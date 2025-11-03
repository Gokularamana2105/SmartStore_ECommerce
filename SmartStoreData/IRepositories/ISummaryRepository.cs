using SmartStoreModels.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.IRepositories
{
    public interface ISummaryRepository:IGenericRepository<Summary>
    {
        Task Update(Summary summary);

        Task<List<Summary>> GetSummaryById(string id);

        Task<List<Summary>> RemoveAllSummary(string userId);
    }
}
