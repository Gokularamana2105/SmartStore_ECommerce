using SmartStoreModels.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.IRepositories
{
    public interface ICartRepository:IGenericRepository<Cart>
    {
        Task<List<Cart>> GetAllCart();

        Task<List<Cart>> GetCartById(string id);

        Task<int> GetCount(Expression<Func<Cart,bool>> filter=null);

        Task<int> GetCountById(string id);

        Task RemoveCart(Guid id);

        Task Update(Cart cart);

        Task<List<Cart>> ActiveApproved(string userId);
    }
}
