using CODEPULSE.API.Data;
using CODEPULSE.API.Models.Domain;
using CODEPULSE.API.Models.DTO;
using CODEPULSE.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CODEPULSE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryRequestDto request)
        {//Map DTO to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            await categoryRepository.CreateAsync(category);
            
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);

        }
        [HttpGet]
        
        public async Task<IActionResult> GetAllCategories([FromQuery] string? query,
            [FromQuery]string? sortBy,
            [FromQuery]string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery]int?pageSize)
        {
           var categories= await categoryRepository.
                GetAllAsync(query,sortBy,sortDirection,pageNumber,pageSize);
            //Map Domain model to DTO
            var response = new List<CategoryDto>();
            foreach(var category in categories)
            {
                response.Add(new CategoryDto 
                { Id = category.Id,
                  Name = category.Name,
                  UrlHandle=category.UrlHandle
                });
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult>GetCategoryById([FromRoute]Guid id)
        {
            var existingCategory=await categoryRepository.GetById(id);
            if(existingCategory == null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(response);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id,UpdateCategoryDto request)
        {
            //Convert DTO to domain model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };
           category= await categoryRepository.UpdateAsync(category);
            var response=new CategoryDto { 
                Id=category.Id,
                Name=category.Name,
                UrlHandle=category.UrlHandle 
            };
            return Ok(response);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category=await categoryRepository.DeleteAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }
    }
}
