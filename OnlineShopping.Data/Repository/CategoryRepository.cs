using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShopping.Data.Repository.IRepository;
using OnlineShopping.Model;

namespace OnlineShopping.Data.Repository 
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
                _context = context;
        }
    }
}
