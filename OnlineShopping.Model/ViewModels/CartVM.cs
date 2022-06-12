using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Model.ViewModels
{
    public class CartVM
    {
        public Order Order { get; set; }
        public IEnumerable<Cart> ListCart { get; set; }
    }
}
