using KeyBoard.Data;
using KeyBoard.DTOs;

namespace KeyBoard.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(Guid id);
        Task<Guid> AddProductAsync(ProductDTO productDto);
        Task UpdateProductAsync(Guid id, ProductDTO productDto);
        Task DeleteProductAsync(Guid id);
    }
}
