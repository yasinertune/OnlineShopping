using OnlineShopping.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork (ApplicationDbContext context)
        {
            _context = context;
        }

        public IAppUserRepository AppUser => new AppUserRepository(_context);

        public ICartRepository Cart =>  new CartRepository(_context);

        public IProductRepository Product =>  new ProductRepository(_context);

        public ICategoryRepository Category =>  new CategoryRepository(_context);

        public IOrderDetailsRepository OrderDetails =>  new OrderDetailsRepository(_context);

        public IOrderRepository Order =>  new OrderRepository(_context);


        public void Dispose()
        {
            //bu sayfada işlemler bittikten sonra Ramden temizliyor 
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
