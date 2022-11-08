using BookStoreWeb.Data;
using BookStoreWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWeb.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public ICategoryRepository CategoryRepository { get; private set; }

        public IAuthorRepository AuthorRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            AuthorRepository = new AuthorRepository(_context);
            ProductRepository = new ProductRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
