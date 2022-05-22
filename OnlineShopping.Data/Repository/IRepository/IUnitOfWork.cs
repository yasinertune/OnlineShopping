using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Data.Repository.IRepository
{
    public interface IUnitOfWork :IDisposable
    {

        IAppUserRepository AppUser { get; }
        ICartRepository Cart { get; }
        IProductRepository Product { get; }

        ICategoryRepository Category { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderRepository Order { get; }

        void Save();

        



    }
}
