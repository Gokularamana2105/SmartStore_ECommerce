using SmartStoreModels.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.IRepositories
{
    public interface IGenericRepository<T> where T:class
    {

        Task<List<T>> GetAll();

        Task<T> FindById(Guid id);
        Task<T> FindId(int id);

        Task<List<T>> GetAllByAsync(Expression<Func<T,bool>> filter);
        Task Add(T entity);

        Task Remove(T entity);

        Task RemoveAll(Expression<Func<T,bool>> filter);

        Task<IEnumerable<T>> Query();

        Task<IEnumerable<T>> QueryAsync(Expression<Func<T,bool>> filter);

        Task<bool> isExist(Expression<Func<T,bool>> filter);


        int Count(Expression<Func<T,bool>> filter=null);


        Task<T>GetRowById(string id);

    }
}
