using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;

namespace KeyBoard.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo , IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            var existing = await _repo.GetCategoryAsync(categoryDTO.Id); // Phải dùng await

            if (existing != null)
            {
                throw new Exception("Category đã tồn tại!");
            }

            var cate = _mapper.Map<Category>(categoryDTO); 
            await _repo.AddCategoryAsync(cate);

            return _mapper.Map<CategoryDTO>(cate); 
        }


        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _repo.GetCategoryAsync(id);

            if (category == null)
            {
                return false; 
            }

            await _repo.DeleteCategoryAsync(id);
            return true;
        }


        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _repo.GetCategoriesAsync();

            if (categories == null || !categories.Any())
            {
                return new List<CategoryDTO>(); 
            }

            return _mapper.Map<List<CategoryDTO>>(categories); 
        }


        public async Task<CategoryDTO> GetCategoryByIdAsync(Guid id)
        {
            var category = await _repo.GetCategoryAsync(id);

            if (category == null) 
            {
                return new CategoryDTO();
            }

            return _mapper.Map<CategoryDTO>(category);
        }


        public async Task<CategoryDTO> UpdateCategoryAsync(Guid id, CategoryDTO categoryDTO)
        {
            var existing = await _repo.GetCategoryAsync(id);

            if (existing == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            _mapper.Map(categoryDTO, existing);

            await _repo.UpdateCategoryAsync(existing);

            return _mapper.Map<CategoryDTO>(existing);
        }

    }
}
