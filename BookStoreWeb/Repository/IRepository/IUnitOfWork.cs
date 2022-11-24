using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWeb.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }

        IProductRepository ProductRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        IApplicationUserRepository ApplicationUserRepository { get; }

        IShoppingCartRepository ShoppingCartRepository { get; }

        IOrderHeaderRepository OrderHeaderRepository { get; }

        IOrderDetailRepository OrderDetailRepository { get; }

        void Save();
    }
}
