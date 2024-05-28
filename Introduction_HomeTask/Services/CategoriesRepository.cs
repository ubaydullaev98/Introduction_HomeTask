using Introduction_HomeTask.Models;

namespace Introduction_HomeTask.Services
{
    public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(NorthwindContext context) : base(context)
        {
        }
    }
}
