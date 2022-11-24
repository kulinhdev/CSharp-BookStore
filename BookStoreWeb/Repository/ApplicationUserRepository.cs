using BookStoreWeb.Data;
using BookStoreWeb.Models;
using BookStoreWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWeb.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Author author)
        {
            _context.Authors.Update(author);
        }
    }
}
