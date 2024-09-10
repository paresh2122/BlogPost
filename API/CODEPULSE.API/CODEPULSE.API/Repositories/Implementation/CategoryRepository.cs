using CODEPULSE.API.Data;
using CODEPULSE.API.Models.Domain;
using CODEPULSE.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CODEPULSE.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        

        public async Task<IEnumerable<Category>> GetAllAsync(string? query = null,
            string? sortBy=null,string? sortDirection=null,
            int? pageNumber=1,
            int? pageSize=10)
        {
            //Query
            var categories = dbContext.Categories.AsQueryable();
            //Filtering
            if (string.IsNullOrWhiteSpace(query) == false)
            {
                categories=categories.Where(x=>x.Name.Contains(query));
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(string.Equals(sortBy,"Name",StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc=string.Equals(sortDirection,"asc",StringComparison.OrdinalIgnoreCase)?true:false;
                    categories=isAsc ?categories.OrderBy(x=>x.Name) :categories.OrderByDescending(x=>x.Name);

                }
                if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? true : false;
                    categories = isAsc ? categories.OrderBy(x => x.UrlHandle) : categories.OrderByDescending(x => x.UrlHandle);

                }
            }

            //Pagination

            var skipResult = (pageNumber - 1) * pageSize;
            categories=categories.Skip(skipResult??0).Take(pageSize??10);

            return await categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory=await dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == category.Id);
            if (existingCategory != null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            return null;

        }
        public async Task<Category> DeleteAsync(Guid id)
        {
           var existingCategory= await dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingCategory is null)
            {
                return null;
            }
            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }
    }
}
