using Introduction_HomeTask.Models;
using System.Linq.Expressions;

namespace Introduction_HomeTask.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly NorthwindContext _context;
        public GenericRepository(NorthwindContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
