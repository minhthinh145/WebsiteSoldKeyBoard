using KeyBoard.DTOs;
using KeyBoard.Helpers;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        //Get List of item
        [HttpGet]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products);
        }

        //Get by id
        [HttpGet("{id}")]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetById(Guid id) 
        {
           var product = await _service.GetProductByIdAsync(id);
           if(product == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }
            return Ok(product);
        }

        //Create a product
        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> CreateNewProduct([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                var product = await _service.AddProductAsync(productDTO);

                if (product == null)
                {
                    return BadRequest("Không thể tạo sản phẩm.");
                }

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        //Update a product
        [HttpPut("{id}")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDTO productDTO)
        {
            if (productDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingProduct = await _service.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            var isUpdated = await _service.UpdateProductAsync(id, productDTO);
            if (!isUpdated)
            {
                return StatusCode(500, "Cập nhật sản phẩm thất bại.");
            }

            return Ok("Cập nhật thành công.");
        }


        //Delete a product
        [HttpDelete("{id}")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            try
            {
                var isDeleted = await _service.DeleteProductAsync(id);
                return isDeleted ? Ok("Xóa sản phẩm thành công.") : NotFound("Không tìm thấy sản phẩm.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }


    }
}
