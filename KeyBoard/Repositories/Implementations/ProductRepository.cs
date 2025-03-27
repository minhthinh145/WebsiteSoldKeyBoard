using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context , IMapper mapper) 
        {
            _context = context;
        }
        public async Task<Product> AddProductAsync(Product Product)
        {
           
            _context.Products!.Add(Product);
            await _context.SaveChangesAsync();
            return Product;
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var listProduct = await _context.Products.ToListAsync();
            return listProduct;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {

            return await _context.Products.FindAsync(id);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var productToUpdate = await _context.Products.FindAsync(product.Id);
            if (productToUpdate == null)
            {
                return false; // Sản phẩm không tồn tại
            }

            _context.Entry(productToUpdate).CurrentValues.SetValues(product); // Cập nhật giá trị mới
            await _context.SaveChangesAsync();
            return true; // Cập nhật thành công
        }

    }
}
