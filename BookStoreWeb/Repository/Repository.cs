
using BookStoreWeb.Data;
using BookStoreWeb.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStoreWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;

        internal DbSet<T> DbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        // Include property: Category
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet.AsQueryable();
            //IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);    
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet.AsQueryable();

            query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveList(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }
    }
}
