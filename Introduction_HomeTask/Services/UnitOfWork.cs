using Introduction_HomeTask.Models;

namespace Introduction_HomeTask.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NorthwindContext _context;
        public UnitOfWork(NorthwindContext context)
        {
            _context = context;
            Categories = new CategoriesRepository(_context);
            Products = new ProductsRepository(_context);
        }
        public ICategoriesRepository Categories { get; private set; }
        public IProductsRepository Products { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        // created here just to not create separate repository for it
        public IEnumerable<Supplier> GetSuppliers()
        {
            return _context.Suppliers;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
