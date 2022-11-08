using BookStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWeb.Repository.IRepository
{
    public interface IProductRepository: IRepository<Product>
    {
        void Update(Product product);
    }
}
