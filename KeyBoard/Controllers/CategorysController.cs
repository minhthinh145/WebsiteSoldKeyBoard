using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategorysController(ICategoryService service)
        {
            _service = service;
        }

        //get list category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categoriesDTO = await _service.GetCategoriesAsync();
            return Ok(categoriesDTO);
        }
        //get category by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id) 
        {
            var category = await _service.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        //Update category
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, CategoryDTO categoryDTO)
        {
            if (categoryDTO.Id != id)
            {
                return BadRequest("ID không khớp.");
            }

            var updatedCategory = await _service.UpdateCategoryAsync(id, categoryDTO);

            if (updatedCategory == null)
            {
                return NotFound("Không tìm thấy danh mục.");
            }

            return Ok(updatedCategory); // Trả về DTO sau khi cập nhật
        }
        //add category
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
        {
            var createdCategory = await _service.AddCategoryAsync(categoryDTO);

            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var result = await _service.DeleteCategoryAsync(id);
            if (!result) // Nếu không tìm thấy category hoặc xóa không thành công
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
