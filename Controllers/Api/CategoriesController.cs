using Microsoft.AspNetCore.Mvc;
using ProductManagement.Repositories;

namespace ProductManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            var result = categories.Select(c => new { id = c.Id, name = c.Name }).ToList();
            return Ok(result);
        }
    }
}
