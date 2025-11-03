using SmartStoreModels.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.IRepositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
         Task Update(Product product);

         Task RemoveProduct(Guid id);

        Task<List<Product>> GetAllProduct();

        Task<List<Product>> GetAllProduct( Guid categoryId, Guid? skipRecord);

        Task<Product> GetProductById(Guid id);
    }
}
