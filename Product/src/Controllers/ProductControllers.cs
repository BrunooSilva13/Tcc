using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Product.Model;  
using Product.src.Service; 

namespace Product.src.Controllersls
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Produto com ID {id} não foi encontrado.");
            }
            return Ok(product);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool created = await _productService.CreateProductAsync(product);
            if (!created)
            {
                return BadRequest("Falha ao criar o produto.");
            }
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != product.Id)
            {
                return BadRequest("ID do produto é inválido.");
            }

            try
            {
                bool updated = await _productService.UpdateProductAsync(product);
                if (!updated)
                {
                    return NotFound($"Produto com ID {id} não foi encontrado.");
                }
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Produto com ID {id} não foi encontrado.");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            bool deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
            {
                return NotFound($"Produto com ID {id} não foi encontrado.");
            }
            return NoContent();
        }
    }
}
