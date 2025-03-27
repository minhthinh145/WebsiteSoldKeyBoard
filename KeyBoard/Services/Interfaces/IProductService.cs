using KeyBoard.Data;
using KeyBoard.DTOs;

namespace KeyBoard.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Lấy danh sách tất cả sản phẩm.
        /// </summary>
        /// <returns>Danh sách sản phẩm dưới dạng DTO.</returns>
        Task<List<ProductDTO>> GetAllProductsAsync();

        /// <summary>
        /// Lấy thông tin của một sản phẩm theo ID.
        /// </summary>
        /// <param name="id">ID của sản phẩm.</param>
        /// <returns>Đối tượng ProductDTO nếu tìm thấy, ngược lại trả về null.</returns>
        Task<ProductDTO?> GetProductByIdAsync(Guid id);

        /// <summary>
        /// Thêm một sản phẩm mới vào hệ thống.
        /// </summary>
        /// <param name="productDTO">Thông tin sản phẩm cần thêm.</param>
        /// <returns>sản phẩm mới được tạo.</returns>
        Task<ProductDTO> AddProductAsync(ProductDTO productDTO);

        /// <summary>
        /// Cập nhật thông tin sản phẩm theo ID.
        /// </summary>
        /// <param name="id">ID của sản phẩm cần cập nhật.</param>
        /// <param name="productDTO">Thông tin mới của sản phẩm.</param>
        /// <returns>True nếu cập nhật thành công, ngược lại false.</returns>
        Task<bool> UpdateProductAsync(Guid id, ProductDTO productDTO);

        /// <summary>
        /// Xóa một sản phẩm theo ID.
        /// </summary>
        /// <param name="id">ID của sản phẩm cần xóa.</param>
        /// <returns>True nếu xóa thành công, ngược lại false.</returns>
        Task<bool> DeleteProductAsync(Guid id);
    }
}
