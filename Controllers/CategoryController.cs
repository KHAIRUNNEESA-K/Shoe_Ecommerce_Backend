using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.DTO.Category;
using ONSTEPS_API.Services.Category;

namespace ONSTEPS_API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _category;
        public CategoryController(ICategoryService category)
        {
            _category = category;
        }
        [HttpPost("Category")]
        public async Task<ActionResult> AddCategory( [FromBody] AddCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _category.AddCategory(categoryDto);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpGet("Category")]
        public async Task<ActionResult> GetCategory()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _category.GetCatogory();
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpPut("UpdateCategory{id}")]
        public async Task<ActionResult> UpdateCategory(int id, CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _category.UpdateCategory(id, category);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
