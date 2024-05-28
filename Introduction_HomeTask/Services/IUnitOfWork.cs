using Introduction_HomeTask.Models;
using Microsoft.EntityFrameworkCore;

namespace Introduction_HomeTask.Services
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoriesRepository Categories { get; }
        IProductsRepository Products { get; }
        int Complete();
        // created here just to not create separate repository for it
        IEnumerable<Supplier> GetSuppliers();
    }
}
