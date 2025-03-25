using KeyBoard.Data;
using KeyBoard.DTOs;

namespace KeyBoard.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(Guid id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Guid id);
        Task SaveChangesAsync();
    }
}
