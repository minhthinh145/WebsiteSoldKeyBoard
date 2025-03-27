using KeyBoard.Data;
using KeyBoard.DTOs;

namespace KeyBoard.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Product> AddProductAsync(Product product); 
        Task<bool> UpdateProductAsync(Product product); 
        Task DeleteProductAsync(Product product);
    }
}
