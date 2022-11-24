using BookStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWeb.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        public int IncrementCount(ShoppingCart shoppingCart, int count);

        public int DecrementCount(ShoppingCart shoppingCart, int count);
        
    }
}
