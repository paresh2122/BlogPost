using CODEPULSE.API.Data;
using CODEPULSE.API.Models.Domain;
using CODEPULSE.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CODEPULSE.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnironment,IHttpContextAccessor httpContextAccessor,ApplicationDbContext dbContext)
        {
           this.webHostEnvironment = webHostEnironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
          return  await dbContext.BlobImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //1-Upload the image in API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream=new FileStream(localPath,FileMode.Create);
            await file.CopyToAsync(stream);
            // 2-Update the database
            var httpRequest=httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;
            await dbContext.BlobImages.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();
            return blogImage;
        }
    }
}
