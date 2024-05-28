using Introduction_HomeTask.Models;

namespace Introduction_HomeTask.Services
{
    public interface IProductsRepository : IGenericRepository<Product>
    {
        IQueryable<Product> GetAllIncludingSupplierAndCategories();
    }
}
