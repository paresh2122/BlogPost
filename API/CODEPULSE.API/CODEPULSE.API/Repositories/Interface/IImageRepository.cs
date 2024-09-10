using CODEPULSE.API.Models.Domain;

namespace CODEPULSE.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
       Task<IEnumerable<BlogImage>> GetAll();
    }
}
