using CODEPULSE.API.Models.Domain;

namespace CODEPULSE.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task <IEnumerable<Category>> GetAllAsync(
            string? query=null,
            string?sortBy=null,
            string? sortDirection=null,
            int? pageNumber=1,
            int?pageSize=10);
        Task <Category?> GetById(Guid id);
        Task<Category> UpdateAsync(Category category);
        Task<Category> DeleteAsync(Guid id);
    }
}
