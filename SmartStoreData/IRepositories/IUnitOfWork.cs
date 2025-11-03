using SmartStoreModels.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStoreData.IRepositories
{
    public interface IUnitOfWork:IDisposable
    {
        Task Save();

        public ICategoryRepository cateogry { get; }

        public IProductRepository prodt { get; }

        public ICartRepository cart { get; }

        public ISummaryRepository summary { get; }

        public IUserRepository user { get; }

        public IOrderRepository order { get; }

        public IOrderSummaryRepository orderSummary { get; }
    }
}
