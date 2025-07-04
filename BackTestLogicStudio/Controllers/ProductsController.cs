using BackTestLogicStudio.Models.Dtos;
using BackTestLogicStudio.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackTestLogicStudio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service) => _service = service;

        [HttpGet("GetCategoriesList")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesList()
        {
            var categories = await _service.GetAllCategories();

            return categories.Count() > 0 ? Ok(categories): NoContent();
        }

        [HttpGet("GetProductsList")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _service.GetAll();

            return products.Count() > 0 ? Ok(products): NoContent();
        }

        [HttpGet("GetProduct/{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _service.GetById(id);

            return product is null ? NotFound() : Ok(product);
        }

        [HttpPut("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductDto product)
        {
            var created = await _service.Create(product);
            return created? Created(): BadRequest();
        }

        [HttpDelete("DeleteProduct/{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _service.Delete(id);

            return deleted ? Ok() : NoContent();
        }

        [HttpPut("UpdateProduct/{id:int}")]
        public async Task<ActionResult<ProductDto>> Update(int id, ProductUpdateDto dto)
        {
            var updated = await _service.Update(id, dto);

            return updated is null ? NotFound() : Ok(updated);
        }
    }
}
