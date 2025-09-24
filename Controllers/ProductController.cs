using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONSTEPS_API.DTO.Product;
using ONSTEPS_API.Services.Category;
using ONSTEPS_API.Services.Product;

namespace ONSTEPS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _services;
        private readonly ICategoryService _category;

        public ProductController(IProductService services, ICategoryService category)
        {
            _services = services;
            _category = category;
            
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("addproduct")]
        public async Task<ActionResult> AddProduct([FromBody] ProductDto addProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _services.AddProduct(addProduct);
            if (response.Success)
            {
                return Ok(response);
            }
            return Unauthorized(response);
        }

        [HttpGet("allProduct")]
        public async Task<ActionResult> AllProduct()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reponse = await _services.GetAllProduct();
            if (reponse.Success)
            {
                return Ok(reponse);
            }
            return BadRequest(reponse);
        }
        [HttpGet("productById")]
        public async Task<ActionResult> ProductByid(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response=await _services.GetProductById(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProduct")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _services.DeleteProduct(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("UpdateProduct{id}")]
        public async Task<ActionResult> Update([FromRoute]int id,[FromBody]ProductUpdateDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _services.UpdateProducts(id,product);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpGet("SearchProduct")]
        public async Task<ActionResult> Search(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest(ModelState);
            }

            var response = await _services.SearchProducts(search);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        
        }
        [HttpGet("Search_By_Category")]
        public async Task<ActionResult> SearchByCategory(string searchByCategory)
        {
           var response= await _category.SearchbyCategory(searchByCategory);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpGet("Pagination")]
        public async Task<ActionResult> Paginations(int pageNumber,int pageSize)
        {
            var response=await _services.PaginationedProduct(pageNumber,pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }

}
