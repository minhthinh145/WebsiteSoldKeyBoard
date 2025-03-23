using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Implementations;
using KeyBoard.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductController(IProductRepository repo)
        {
            _repo = repo;
        }

        //Get List of item
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repo.GetAllProductsAsync();
            return Ok(products);
        }

        //Get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) 
        {
            var product = await _repo.GetProductByIdAsync(id);
            if (product == null) 
            {
                return NotFound();
            }
            return Ok(product);
        }

        //Create a product
        [HttpPost]
        public async Task<IActionResult> CreateNewProduct([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                var _newKB = await _repo.AddProductAsync(productDTO);
                var kb = await _repo.GetProductByIdAsync(_newKB);
                return kb == null ? NotFound() : Ok(kb);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }


        //Update a product
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest("Id không khớp với dữ liệu đầu vào.");
            }
            if (productDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var product = await _repo.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            await _repo.UpdateProductAsync(id, productDTO);
            return Ok("Cập nhật thành công.");
        }


        //Delete a product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            await _repo.DeleteProductAsync(id);
            return Ok("Xoá thành công.");
        }

    }
}
