using BookStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWeb.Repository.IRepository
{
    public interface IAuthorRepository : IRepository<Author>
    {
        void Update(Author product);
    }
}
