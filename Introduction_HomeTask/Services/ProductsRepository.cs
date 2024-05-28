using Introduction_HomeTask.Models;
using Microsoft.EntityFrameworkCore;

namespace Introduction_HomeTask.Services
{
    public class ProductsRepository : GenericRepository<Product>, IProductsRepository
    {
        public ProductsRepository(NorthwindContext context) : base(context)
        {
        }

        public IQueryable<Product> GetAllIncludingSupplierAndCategories()
        {
            return _context.Products.Include(p => p.Category).Include(p => p.Supplier).OrderBy(p => p.ProductId);
        }
    }
}
