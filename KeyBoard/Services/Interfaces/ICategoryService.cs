using KeyBoard.DTOs;

namespace KeyBoard.Services.Interfaces
{
    public interface ICategoryService
    {

        /// <summary>
        /// Lấy danh sách tất cả danh mục.
        /// </summary>
        /// <returns>Danh sách các CategoryDTO.</returns>
        Task<List<CategoryDTO>> GetCategoriesAsync();

        /// <summary>
        /// Lấy danh mục theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục.</param>
        /// <returns>CategoryDTO nếu tìm thấy, ngược lại trả về null.</returns>
        Task<CategoryDTO> GetCategoryByIdAsync(Guid id);

        /// <summary>
        /// Cập nhật thông tin danh mục.
        /// </summary>
        /// <param name="id">ID của danh mục cần cập nhật.</param>
        /// <param name="categoryDTO">Dữ liệu danh mục mới.</param>
        /// <returns>CategoryDTO đã được cập nhật.</returns>
        Task<CategoryDTO> UpdateCategoryAsync(Guid id, CategoryDTO categoryDTO);

        /// <summary>
        /// Thêm mới một danh mục.
        /// </summary>
        /// <param name="categoryDTO">Thông tin danh mục cần thêm.</param>
        /// <returns>CategoryDTO đã được thêm.</returns>
        Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDTO);

        /// <summary>
        /// Xóa danh mục theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục cần xóa.</param>
        /// <returns>True nếu xóa thành công, ngược lại false.</returns>
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}
