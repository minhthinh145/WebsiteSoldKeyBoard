using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategorysController(ICategoryRepository _repo, IMapper mapper)
        {
            this._repo = _repo;
            _mapper = mapper;
        }

        //get list category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _repo.GetCategoriesAsync();
            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);
            return Ok(categoriesDTO);
        }
        //get category by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id) 
        {
           var category = await _repo.GetCategoryAsync(id);
           var categoryDTO = _mapper.Map<CategoryDTO>(category);
           return Ok(categoryDTO);
        }

        //Update category
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id,CategoryDTO categoryDTO) 
        {
            if (categoryDTO.Id != id) 
            {
                return BadRequest("ID không khớp");
            }
            //mapper to Model
            var categoryModel = _mapper.Map<Category>(categoryDTO);
            try
            {
                await _repo.UpdateCategoryAsync(categoryModel);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
            var updateCate = _mapper.Map<CategoryDTO>(categoryModel);
            return Ok(updateCate);
        }

        //add category
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO) 
        {
            var categoryModel = _mapper.Map<Category>(categoryDTO);
            //dont need to create a new id because entityframework do it
            await _repo.AddCategoryAsync(categoryModel);
            return Ok(categoryDTO);
        }

        //delete category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id) 
        {
            try
            {
                var category = await _repo.GetCategoryAsync(id);
                if (category == null)
                {
                    return BadRequest("Đã delete hoặc k tìm thấy category");
                }
                await _repo.DeleteCategoryAsync(id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
