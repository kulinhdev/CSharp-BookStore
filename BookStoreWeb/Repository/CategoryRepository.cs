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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
